using System;
using System.IO;
using Docker_app.Dapp.Docker_runner;
using Docker_app.Dapp.Structures;

namespace Docker_app.Dapp.Desktop
{
  public class Desktop
  {
    private string UserDir => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string DesktopFilesDir => UserDir + "/.local/share/applications";
    public string Prefix { get; private set; }

    private readonly string _runnerPath;

    private const string ExePattern = "<exe>";

    public string DesktopFilename(string container, string app)
      => $"{DesktopFilesDir}/{Prefix}-{container}-{app}.desktop";

    public Desktop(DappConfig config, string runnerPath)
    {
      Prefix = config.DesktopPrefix;
      _runnerPath = runnerPath;
    }

    private string GetExecPath(string containerName, string appName)
      => new ParamsBuilder() | _runnerPath | "-r" | $"{containerName}/{appName}";


    public void Create(DockerApp app)
    {
      var containerName = app.Container.Name;
      var appName = app.Name;
      var desktopConfig = app.Desktop;

      string icon = null;
      string terminal = null;
      string comment = null;
      string name = null;

      var runnerPath = GetExecPath(containerName, appName);
      if (desktopConfig != null)
      {
        name = desktopConfig.name;

        comment = desktopConfig.comment;


        if (!string.IsNullOrEmpty(desktopConfig.icon))
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
        if (!string.IsNullOrEmpty(desktopConfig.terminal))
        {
          string param = new ParamsBuilder(true) | runnerPath;
          terminal = desktopConfig.terminal.Replace(ExePattern, param);
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

          w.WriteLine("DappApp=" + (terminal ?? runnerPath));
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