using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Docker_app.Dapp.Desktop
{
  public class DesktopFile
  {
    public string Version { get; set; }
    public string Exec { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
  }
}