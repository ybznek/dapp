using System;

namespace Docker_app.Dapp.Docker_runner
{
  public class DockerException : ApplicationException
  {
    public int ExitCode { get; private set; }

    public DockerException(string parameters, int exitCode)
      : base(parameters)
    {
      ExitCode = exitCode;
    }
  }
}