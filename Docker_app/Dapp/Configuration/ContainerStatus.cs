using System.ComponentModel;

namespace Docker_app.Dapp.Configuration
{
  public enum ContainerStatus
  {
    [Description("Not exists")] NotExists,
    Stopped,
    Running
  }
}