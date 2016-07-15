using System.Collections.Generic;
using System.IO;
using System.Linq;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Structures
{
  public class DockerApps
  {
    private const string XmlConfig = "config.xml";
    private const string JsonConfig = "config.json";
    public IEnumerable<DockerContainer> Containers => _Containers();

    public IEnumerable<DockerApp> Apps =>
      from container in Containers
      from app in container.Apps
      select new DockerApp(container, app.Key);

    private DappConfig _config;

    private string _appsDir;

    public DockerApps(DappConfig config)
    {
      Directory.CreateDirectory(config.AppsDir);

      _appsDir = config.AppsDir;
      _config = config;
    }

    private IEnumerable<DockerContainer> _Containers()
    {
      var configs = new[] {XmlConfig, JsonConfig};

      foreach (var path in Directory.EnumerateDirectories(_appsDir))
      {
        if (File.Exists(Path.Combine(path, "Dockerfile")))
        {
          foreach (var config in configs)
          {
            var configFile = Path.Combine(path, config);
            if (File.Exists(configFile))
            {
              yield return new DockerContainer(path, config, _config.ConstList);
              break;
            }
          }
        }
      }
    }

    public IEnumerable<DockerApp> GetApps(string name)
    {
      var targetAppName = new DockerAppName(name);

      return
        from container in Containers
        from app in container.Apps
        let appName = app.Key
        where targetAppName.Equals(container.Name, appName)
        select new DockerApp(container, app.Key);
    }

    public void GenerateTemplate(string appName)
    {
      var appPath = Path.Combine(_config.AppsDir, appName);
      Directory.CreateDirectory(appPath);

      var dockerFilePath = Path.Combine(appPath, "Dockerfile");
      File.WriteAllText(dockerFilePath, "FROM scratch\n");

      var configFilePath = Path.Combine(appPath, XmlConfig);
      ContainerConfig.MakeTemplate(configFilePath);
    }
  }
}