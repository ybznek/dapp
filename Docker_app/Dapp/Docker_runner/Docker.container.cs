using System.Text;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    public ContainerStatus GetContainerStatus(string containerName)
    {
      var args = Params()
                 | "ps"
                 | "-a"
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

    public void RemoveContainer(string containerName) => RunNow(Params() | "rm" | containerName);

    public void KillContainer(string containerName) => RunNow(Params() | "kill" | containerName);

    protected void RunContainer(string containerName, string name, string imageName)
    {
      var args = Params()
                 | "run"
                 | "-tid"
                 | "-v" | "/etc/passwd:/etc/passwd:ro"
                 | "-v" | "/etc/group:/etc/group:ro"
                 | "-v" | "/p:/p"
                 | "-v" | "/home:/home"
                 | "-v" | "/root:/root"
                 | "-v" | "/tmp:/tmp"
                 | "--name" | containerName
                 | "--hostname" | GetHostname(containerName)
                 | imageName
                 | "cat";

      RunNow(args);
    }
  }
}