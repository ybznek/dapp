using System.Collections.Generic;

namespace Docker_app.Dapp.Configuration
{
  public class Exec : IExec
  {
    public string Cmd => cmd;
    public string User => user;
    public bool ExecProcess => exec_process;

    public Dictionary<string, string> Desktop => desktop;
    public IEnumerable<string> Flags => flags;
    public string cmd;
    public string user = null;
      public Dictionary<string, string> desktop;
    public bool exec_process = false;
    public string[] flags;

    public Exec(string cmd)
    {
      this.cmd = cmd;
    }

    public static implicit operator Exec(string cmd)
    {
      return new Exec(cmd);
    }
  }
}