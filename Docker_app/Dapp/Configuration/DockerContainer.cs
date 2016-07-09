using System.Collections.Generic;
using System.IO;

namespace Docker_app.Dapp.Configuration
{
   public class DockerContainer
    {
      public string ContainerPath { get; }

      public string Name => new DirectoryInfo(ContainerPath).Name;

      private string ConfigPath => Path.Combine(ContainerPath, "config.json");

      private string DockerfilePath => Path.Combine(ContainerPath, "Dockerfile");

      public DockerConfig Config => new DockerConfig(ConfigPath);

      public string Hash
      {
        get
        {
          var cf = new FileInfo(ConfigPath).LastWriteTimeUtc;
          var df = new FileInfo(DockerfilePath).LastWriteTimeUtc;

          return (cf + "_" + df);
        }
      }

      public IEnumerable<KeyValuePair<string, string>> Apps => Config.GetExecList();

      public DockerContainer(string containerPath)
      {
        ContainerPath = containerPath;
      }
    }
}