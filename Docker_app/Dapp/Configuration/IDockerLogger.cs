using System.Security.Cryptography.X509Certificates;

namespace Docker_app.Dapp.Configuration
{
  public interface IDockerLogger
  {
    void Command(string cmd, bool important = false);
    void Output(string msg, bool important = false);
    void Exit(string msg, bool important = true);
    void Dockerfile(string msg, bool important = true);
  }
}