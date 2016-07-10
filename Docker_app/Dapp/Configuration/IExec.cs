using System.Collections.Generic;

namespace Docker_app.Dapp.Configuration
{
  public interface IExec
  {
    string Cmd { get; }
    string User { get; }
    bool ExecProcess { get; }
    IEnumerable<string> Flags { get; }
  }
}