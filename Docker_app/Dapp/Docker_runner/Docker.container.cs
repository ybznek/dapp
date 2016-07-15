using System.Linq;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    public ContainerStatus GetContainerStatus(string containerName)
    {
      var args = Params()
                 | "ps" | "-a"
                 | $"--filter=name={containerName}"
                 | "--format"
                 | "{{.Status}}"
        ;

      var output = RunNowOutput(args);

      if (string.IsNullOrEmpty(output))
      {
        return ContainerStatus.NotExists;
      }
      else if (output.StartsWith("Up"))
      {
        return ContainerStatus.Running;
      }
      else
      {
        return ContainerStatus.Stopped;
      }
    }

    public void RemoveContainer(string containerName)
      => RunNow(Params() | "rm" | "-f" | containerName, important: false);

    public void KillContainer(string containerName) => RunNow(Params() | "kill" | containerName, important: false);

    protected void RunContainer(string containerName, ContainerConfig config, string name, string imageName)
    {
      var args = Params()
                 | "run"
                 | "-tid";
      config.Mounts?.Aggregate(args,
        (current, mount)
          => current | "-v" | $"{mount.Host}:{mount.Container}:{mount.Mode}");

      config.Flags?.Aggregate(args, (current, flag) => current | flag);

      args = args
             | "--name" | containerName
             | "--hostname" | GetHostname(containerName)
             | imageName
             | "cat";

      RunNow(args, important: false);
    }
  }
}