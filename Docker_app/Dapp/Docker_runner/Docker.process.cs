using System.Diagnostics;
using System.Text;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    private const string DockerCmd = "docker";

    protected Process Run(string param, string exec = DockerCmd, bool stdin = true, bool stdout = true,
      bool stderr = true,
      bool shellExec = false)
    {
      Logger.Command(param);
      return ProcessHelper.Run(exec, param, stdin, stdout, stderr, shellExec);
    }


    /// <exception cref="DockerException">Exit code is not 0</exception>
    protected void RunNow(string param, string exec = DockerCmd)
    {
      var process = Run(param, exec);
      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line);
      }
      process.WaitForExit();
      if (process.ExitCode != 0)
      {
        throw new DockerException(param, process.ExitCode);
      }
    }

    /// <exception cref="DockerException">Exit code is not 0</exception>
    protected string RunNowOutput(string param, string exec = DockerCmd)
    {
      var process = Run(param, exec);
      var b = new StringBuilder();
      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line);
        b.Append(line);
      }
      process.WaitForExit();
      if (process.ExitCode != 0)
      {
        throw new DockerException(param, process.ExitCode);
      }
      return b.ToString();
    }


    protected Process Run(ParamsBuilder param, string exec = DockerCmd, bool stdin = true, bool stdout = true,
        bool stderr = true,
        bool shellExec = false)
      => Run(param.ToString(), exec, stdin, stdout, stderr, shellExec);

    protected void RunNow(ParamsBuilder param, string exec = DockerCmd)
      => RunNow(param.ToString(), exec);

    protected string RunNowOutput(ParamsBuilder param, string exec = DockerCmd)
      => RunNowOutput(param.ToString(), exec);
  }
}