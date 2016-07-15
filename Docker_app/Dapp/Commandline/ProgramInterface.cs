using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Docker_app.Dapp.Configuration;
using Docker_app.Dapp.Docker_runner;
using Docker_app.Dapp.Structures;

namespace Docker_app.Dapp.Commandline
{
  public class ProgramInterface
  {
    private readonly Options opts = new Options();
    private readonly bool parsed;

    private Docker Docker => _dockerService.Obtain();

    private DockerService _dockerService { get; set; }

    private string _runnable;

    public ProgramInterface(string[] args, string runnable)
    {
      var parser = new CommandLineParser();
      parsed = parser.ParseArguments(args, opts);
      _runnable = runnable;
    }

    public void Run()
    {
      var dapCfg = new DappConfig("/tmp/config");
      dapCfg.Load();
      var logger = new DockerLogger(opts.Verbose);
      _dockerService = new DockerService(dapCfg, logger);
      try
      {
        if (parsed)
        {
          var apps = new DockerApps(dapCfg);

          // Generate template
          if (!string.IsNullOrEmpty(opts.GenerateApp))
          {
            apps.GenerateTemplate(opts.GenerateApp);
          }

          // Applications
          if (opts.List)
          {
            Console.WriteLine("Applications:");
            foreach (var p in apps.Apps)
            {
              Console.WriteLine("|>>" + p.Name);
            }
            Console.WriteLine("NMew");
            var appsEnumerable = PrepareApps(apps.Apps);
            TableFormatter.Format(Console.Error, appsEnumerable, "\t{0}\t({1})\n");
            return;
          }

          // Run
          if (!string.IsNullOrEmpty(opts.App))
          {
            RunApp(apps, opts.App, opts.RunOpts);
            return;
          }

          // Details
          if (!string.IsNullOrEmpty(opts.Details))
          {
            ShowApp(apps, opts.Details);
            return;
          }

          // Desktop file
          if (!string.IsNullOrEmpty(opts.CreateDesktop))
          {
            var d = new Desktop.Desktop(dapCfg, _runnable);
            CreateDesktop(d, apps, opts.CreateDesktop);
            return;
          }

          Console.WriteLine(opts.GetUsage());
        }
        else
        {
          // Parse error
          Console.WriteLine(opts.GetUsage());
        }
      }
      catch (DockerException ex)
      {
        logger.Exit($"Exited with exit code ({ex.ExitCode}): docker {ex.Message}");
      }
    }

    private IEnumerable<Tuple<string, string>> PrepareApps(IEnumerable<DockerApp> apps) =>
      from app in apps
      let path = app.Container.ContainerPath
      select new Tuple<string, string>($"{app.Container.Name}/{app.Name}", path);

    private bool CheckApps(IReadOnlyCollection<DockerApp> toRun)
    {
      // 0    apps
      if (toRun.Count == 0)
      {
        Console.Error.WriteLine("No such app.");
      }

      // >=2  apps
      else if (toRun.Count >= 2)
      {
        Console.Error.WriteLine("Multiple apps found.");
        foreach (var f in toRun)
        {
          Console.Error.WriteLine("{0}/{1}", new object[] {f.Container.Name, f.Name});
        }
      }
      else
      {
        // 1 app
        return true;
      }
      return false;
    }

    private void RunApp(DockerApps apps, string appName, RunOptions options = RunOptions.None)
    {
      var toRun = apps.GetApps(appName).ToArray();

      if (!CheckApps(toRun)) return;

      var app = toRun[0];
      var docker = Docker;
      docker.RunDockerApp(app, options);
    }

    private void CreateDesktop(Desktop.Desktop desktop, DockerApps apps, string appName)
    {
      var toRun = apps.GetApps(appName).ToArray();

      if (!CheckApps(toRun)) return;

      var app = toRun[0];
      desktop.Create(app);
    }

    private void ShowApp(DockerApps apps, string appName)
    {
      var toRun = apps.GetApps(appName).ToArray();

      if (!CheckApps(toRun)) return;

      var app = toRun[0];
      var container = app.Container;

      // DockerContainer
      Console.Out.WriteLine("DockerContainer: ");
      var docker = Docker;
      var status = docker.GetContainerStatus(docker.GetContainerName(container.Name));
      Console.Out.Write(string.Format($"\t{container.Name} ({status})\n"));

      // DockerContainer Path
      Console.Out.WriteLine("Path: ");
      Console.Out.Write(string.Format($"\t{container.ContainerPath}\n"));

      // Mounts
      Console.Out.WriteLine("Mounts: ");
      foreach (var mount in container.Config.Mounts)
      {
        Console.Out.Write(string.Format($"\t{mount.Mode.ToUpper()}: {mount.Host} => {mount.Container}\n"));
      }
    }
  }
}