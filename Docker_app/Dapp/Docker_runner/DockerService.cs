using System;

namespace Docker_app.Dapp.Docker_runner
{
  public class DockerService
  {
    public Docker Obtain()
    {
      if (_docker != null) return _docker;

      _docker = new Docker(_cfg, Logger);

#if DEBUG
      var version = _docker.GetVersion();
      Console.Error.WriteLine(string.Format($"Docker version: {version}"));
#endif
      return _docker;
    }


    private Docker _docker = null;

    private IDockerLogger Logger { get; }

    private DappConfig _cfg;

    public DockerService(DappConfig cfg, IDockerLogger logger)
    {
      _cfg = cfg;
      Logger = logger;
    }
  }
}