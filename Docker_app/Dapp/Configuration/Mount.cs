using System.Runtime.Serialization;

namespace Docker_app.Dapp.Configuration
{
  [DataContract]
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

    [DataMember(Name = "host")]
    public string host;

    [DataMember(Name = "container")]
    public string container;

    [DataMember(Name = "mode")]
    public string mode
    {
      get { return _mode == MountMode.Rw ? "rw" : "ro"; }
      set { _mode = value.Equals("rw") ? MountMode.Rw : MountMode.Ro; }
    }

    private MountMode _mode;
  }
}