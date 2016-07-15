using System.Collections.Generic;
using System.IO;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Structures
{
  public class DockerContainer
  {
    public string ContainerPath { get; }
    public string Name => new DirectoryInfo(ContainerPath).Name;
    private string ConfigPath => _configFile;
    private string DockerfilePath => Path.Combine(ContainerPath, "Dockerfile");
    private readonly IReadOnlyDictionary<string, string> _constants;
    public ContainerConfig Config => new ContainerConfig(ConfigPath, _constants);
    public IEnumerable<KeyValuePair<string, IDappApp>> Apps => Config.GetExecList();
    private readonly string _configFile;

    public DockerContainer(string containerPath, string configFile, IReadOnlyDictionary<string, string> constants)
    {
      ContainerPath = containerPath;
      _constants = constants;
      _configFile = Path.Combine(ContainerPath, configFile);
    }

    public string Hash
    {
      get
      {
        var cf = new FileInfo(ConfigPath).LastWriteTimeUtc;
        var df = new FileInfo(DockerfilePath).LastWriteTimeUtc;
        return (cf + "_" + df);
      }
    }
  }
}