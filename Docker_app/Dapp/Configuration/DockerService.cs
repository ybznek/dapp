using System;
using Docker_app.Dapp.Docker_runner;

namespace Docker_app.Dapp.Configuration
{
  public class DockerService
  {
    public Docker Obtain()
    {
      if (_docker != null) return _docker;

      _docker = new Docker(Prefix, Logger);

#if DEBUG
      var version = _docker.GetVersion();
      Console.Error.WriteLine(string.Format($"Docker version: {version}"));
#endif
      return _docker;
    }


    private Docker _docker = null;

    private IDockerLogger Logger { get; }
    private string Prefix { get; }

    public DockerService(string prefix, IDockerLogger logger)
    {
      Prefix = prefix;
      Logger = logger;
    }
  }
}