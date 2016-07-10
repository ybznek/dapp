using System.ComponentModel;

namespace Docker_app.Dapp.Configuration
{
  public class Mount : IMount
  {
    public string Host => host;
    public string Container => container;
    public string Mode => mode;

    private enum MountMode : byte
    {
      Rw,
      Ro
    }

    public string host;
    public string container;

    public string mode
    {
      get { return _mode == MountMode.Rw ? "rw" : "ro"; }
      set { _mode = value.Equals("rw") ? MountMode.Rw : MountMode.Ro; }
    }

    private MountMode _mode;
  }
}