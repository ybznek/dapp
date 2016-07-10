using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using Docker_app.Dapp.Configuration;
using Docker_app.Dapp.Docker_runner;

namespace Docker_app.Dapp.Desktop
{
  static class DictHelper
  {
    public static string SafeGet(this Dictionary<string, string> dict, string key)
    {
      string val;

      return
        dict.TryGetValue(key, out val)
          ? (!string.IsNullOrEmpty(val) ? val : null)
          : null;
    }
  }

  public class Desktop
  {
    private string UserDir => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string DesktopFilesDir => UserDir + "/.local/share/applications";
    public string Prefix { get; private set; }

    private readonly string _runnerPath;

    private readonly string exePattern = "<EXE>";

    public string DesktopFilename(string container, string app)
      => $"{DesktopFilesDir}/{Prefix}-{container}-{app}.desktop";

    public Desktop(string prefix, string runnerPath)
    {
      Prefix = prefix;
      _runnerPath = runnerPath;
    }

    private string GetExecPath(string containerName, string appName)
      => new ParamsBuilder() | _runnerPath | "-r" | $"{containerName}/{appName}";


    public void create(DockerApp app)
    {
      var containerName = app.Container.Name;
      var appName = app.Name;
      var desktopConfig = app.Desktop;

      string icon = null;
      string terminal = null;
      string comment = null;
      string name = null;

      string runnerPath = GetExecPath(containerName, appName);
      if (desktopConfig != null)
      {
        name = desktopConfig.SafeGet("name");
        comment = desktopConfig.SafeGet("comment");

        if (desktopConfig.TryGetValue("icon", out icon))
        {
          if (!string.IsNullOrEmpty(icon))
          {
            switch (icon[0])
            {
              case '.': // relative
                icon = Path.Combine(app.Container.ContainerPath, icon); // make it absolute
                break;
              case '/': // absolute
                break;
              default: // freedesktop
                break;
            }
          }
        }
        if (desktopConfig.TryGetValue("terminal", out terminal))
        {
          if (terminal != null)
          {
            string param = new ParamsBuilder(true) | runnerPath;
            terminal = terminal.Replace(exePattern, param);
          }
        }
      }


      var filename = DesktopFilename(containerName, appName);
      using (var f = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write))
      {
        using (var w = new StreamWriter(f))
        {
          w.WriteLine("[Desktop Entry]");
          w.WriteLine("Encoding=UTF-8");
          w.WriteLine("Type=Application");
          w.WriteLine("Version=1.0");

          w.WriteLine("Comment=" + (comment ?? "DockerApp app"));
          w.WriteLine($"Name=" + (name ?? appName));

          w.WriteLine("Exec=" + (terminal ?? runnerPath));
          if (icon != null)
          {
            w.WriteLine($"Icon={icon}");
          }
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