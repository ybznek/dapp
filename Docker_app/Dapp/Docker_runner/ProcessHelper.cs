using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Docker_app.Dapp.Docker_runner
{
  public static class ProcessHelper
  {
    [DllImport("libc.so.6")]
    private static extern int execvp(string path, string[] argv);


    static IEnumerable<string> Getargs(string program, IEnumerable<string> args)
    {
      yield return program;
      foreach (var arg in args)
      {
        yield return arg;
      }
      yield return null;
    }

    public static int Exec(string program, IEnumerable<string> args)
    {
      var preparedArgs = Getargs(program, args);
      return execvp(program, preparedArgs.ToArray());
    }

    public static Process Run(
      string filename,
      ParamsBuilder param,
      bool stdin = true,
      bool stdout = true,
      bool stderr = true,
      bool shellExec = false,
      bool exec = false)
    {
      var processStartInfo = new ProcessStartInfo
      {
        FileName = filename,
        Arguments = param,
        RedirectStandardInput = stdin,
        RedirectStandardOutput = stdout,
        RedirectStandardError = stderr,
        UseShellExecute = shellExec
      };

      return Process.Start(processStartInfo);
    }

    public static IEnumerable<string> ReadOutputLines(this Process process)
    {
      using (var reader = process.StandardOutput)
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          yield return line;
        }
      }
    }
  }
}