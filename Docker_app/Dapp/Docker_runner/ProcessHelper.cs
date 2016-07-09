using System.Collections.Generic;
using System.Diagnostics;

namespace Docker_app.Dapp.Docker_runner
{
  public static class ProcessHelper
  {
    public static Process Run(
      string filename,
      string param = "",
      bool stdin = true,
      bool stdout = true,
      bool stderr = true,
      bool shellExec = false)
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