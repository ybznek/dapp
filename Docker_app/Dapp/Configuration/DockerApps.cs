using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Docker_app.Dapp.Configuration
{
  public class DockerApps
  {
    public string[] SearchPaths { get; private set; }

    public IEnumerable<DockerContainer> Containers => _Containers();

    public IEnumerable<DockerApp> Apps => from container in Containers
      from app in container.Apps
      select new DockerApp(container, app.Key);

    public DockerApps(string[] searchPaths)
    {
      SearchPaths = searchPaths;
    }

    private IEnumerable<DockerContainer> _Containers()
    {
      return
        from path in SearchPaths
        from containerPath in Directory.EnumerateDirectories(path)
        where
        (
          File.Exists(Path.Combine(containerPath, "Dockerfile"))
          &&
          File.Exists(Path.Combine(containerPath, "config.json"))
        )
        select new DockerContainer(containerPath);
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
  }
}