using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker_app.Dapp.Configuration
{
  [DataContract]
  public class DappApp : IDappApp
  {
    public string Cmd => cmd;
    public string User => user;
    public bool ExecProcess => exec_process;
    public DesktopConfig Desktop => desktop;

    public IEnumerable<string> Flags => flags;

    [DataMember(Name="command")] public string cmd;
    [DataMember(Name="user")] public string user = null;
    [DataMember(Name="exec")] public bool exec_process = false;
    [DataMember(Name="flags")] public FlagsList flags;
    [DataMember(Name="desktop")] public DesktopConfig desktop;

    public DappApp(string cmd)
    {
      this.cmd = cmd;
    }

    public static implicit operator DappApp(string cmd)
    {
      return new DappApp(cmd);
    }
  }
}