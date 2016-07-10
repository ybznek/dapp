using System;
using System.Diagnostics;
using System.Text;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    private const string DockerCmd = "docker";

    protected Process Run(ParamsBuilder param, string exec = DockerCmd, bool stdin = true, bool stdout = true,
      bool stderr = true,
      bool shellExec = false)
    {
      Logger.Command(param);
      return ProcessHelper.Run(exec, param, stdin, stdout, stderr, shellExec);
    }

    protected void Exec(ParamsBuilder args)
    {
      Logger.Command(args,false);
      var exit = ProcessHelper.Exec(DockerCmd, args.Params);
      if (exit != 0)
      {
        throw new DockerException(args, exit);
      }
      return;
    }

    /// <exception cref="DockerException">Exit code is not 0</exception>
    protected void RunNow(ParamsBuilder param, string exec = DockerCmd, bool important = false)
    {
      var process = Run(param, exec);
      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line, important);
      }
      process.WaitForExit();
      if (process.ExitCode != 0)
      {
        throw new DockerException(param, process.ExitCode);
      }
    }

    /// <exception cref="DockerException">Exit code is not 0</exception>
    protected string RunNowOutput(ParamsBuilder param, string exec = DockerCmd, bool important = false)
    {
      var process = Run(param, exec);
      var b = new StringBuilder();
      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line, important);
        b.Append(line);
      }
      process.WaitForExit();
      if (process.ExitCode != 0)
      {
        throw new DockerException(param, process.ExitCode);
      }
      return b.ToString();
    }
  }
}