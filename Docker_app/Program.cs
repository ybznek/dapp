using System;
using System.IO;
using System.Reflection;
using Docker_app.Dapp.Commandline;

namespace Docker_app
{
  class Program
  {
    static void Main(string[] args)
    {

#if (DEBUG)
      var time = new FileInfo(Assembly.GetExecutingAssembly().Location).CreationTime;
      Console.Error.WriteLine($"Debug Build time: {time}");
#endif
      var iface = new ProgramInterface(args);

      iface.Run();
    }
  }
}