using System.Collections.Generic;

namespace Docker_app.Dapp.Configuration
{
  public interface IDappApp
  {
    string Cmd { get; }
    string User { get; }
    bool ExecProcess { get; }

    DesktopConfig Desktop { get; }
    IEnumerable<string> Flags { get; }
  }
}