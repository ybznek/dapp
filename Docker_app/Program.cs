using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Docker_app.Dapp.Commandline;
using Docker_app.Dapp.Desktop;

namespace Docker_app
{
  static class Program
  {
    static void Main(string[] args)
    {

/*      foreach (var f in       d.getFiles())
      {
        Console.WriteLine(f.Filename);
      }
*/
      var runnable = Assembly.GetExecutingAssembly().Location;
      Debug.Assert(runnable != null);
#if (DEBUG)
      var time = new FileInfo(runnable).CreationTime;
      Console.Error.WriteLine($"Debug Build time: {time}");
#endif
      var iface = new ProgramInterface(args,runnable);

      iface.Run();
    }
  }
}