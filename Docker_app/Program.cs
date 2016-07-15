using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Docker_app.Dapp.Commandline;

namespace Docker_app
{
  static class Program
  {
    static void Main(string[] args)
    {
      var c = new DappConfig("/tmp/lol/f");
      c.Load();
      var runnable = Assembly.GetExecutingAssembly().Location;
      Debug.Assert(runnable != null);
#if (DEBUG)
      var time = new FileInfo(runnable).CreationTime;
      Console.Error.WriteLine($"Debug Build time: {time}");
#endif
      var iface = new ProgramInterface(args, runnable);

      iface.Run();
    }
  }
}