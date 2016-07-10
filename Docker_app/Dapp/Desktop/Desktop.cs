using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Desktop
{
  public class Desktop
  {
    private string UserDir => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string DesktopFilesDir => UserDir + "/.local/share/applications";
    public string Prefix { get; private set; }

    private readonly string _runnerPath;

    public string DesktopFilename(string container, string app)
      => $"{DesktopFilesDir}/{Prefix}-{container}-{app}.desktop";

    public Desktop(string prefix, string runnerPath)
    {
      Prefix = prefix;
      _runnerPath = runnerPath;
    }

    public void create(DockerApp app)
    {
      var containerName = app.Container.Name;
      var appName = app.Name;

      var filename = DesktopFilename(containerName, appName);
      using (var f = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write))
      {
        using (var w = new StreamWriter(f))
        {
          w.WriteLine("[Desktop Entry]");
          w.WriteLine("Encoding=UTF-8");
          w.WriteLine("Type=Application");
          w.WriteLine("Version=1.0");

          w.WriteLine("Comment=" + "DockerApp app");
          w.WriteLine($"Name={appName}");
          w.WriteLine($"Exec={_runnerPath} -r {containerName}/{appName}");
        }
      }
    }

    /*public IEnumerable<DesktopFile> getFiles()
    {
      foreach (var file in Directory.GetFiles(DesktopFilesDir))
      {
        yield return new DesktopFile(file);
      }
    }*/
  }
}