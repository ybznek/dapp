using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Docker_app.Dapp.Configuration;
using Docker_app.Dapp.Docker_runner;

namespace Docker_app.Dapp.Commandline
{
  public class ProgramInterface
  {
    private readonly Options opts = new Options();
    private readonly bool parsed;

    private Docker Docker => _dockerService.Obtain();

    private DockerService _dockerService { get; set; }

    public ProgramInterface(string[] args)
    {
      var parser = new CommandLineParser();
      parsed = parser.ParseArguments(args, opts);
    }

    public void Run()
    {
      var logger = new DockerLogger(opts.Verbose);
      _dockerService = new DockerService("DOCKER_APP", logger);
      try
      {
        if (parsed)
        {
          var apps = new DockerApps(new[] {"/home/data/projects/docker/apps/"});

          // Help
          if (opts.Help)
          {
            Console.WriteLine(opts.GetUsage());
          }

          // Applications
          if (opts.List)
          {
            Console.WriteLine("Applications:");

            var appsEnumerable = PrepareApps(apps.Apps);
            TableFormatter.Format(Console.Error, appsEnumerable, "\t{0}\t({1})\n");
          }

          // Run
          if (!string.IsNullOrEmpty(opts.App))
          {
            RunApp(apps, opts.App, opts.RunOpts);
          }

          // Details
          if (!string.IsNullOrEmpty(opts.Details))
          {
            ShowApp(apps, opts.Details);
          }
        }
        else
        {
          // Parse error
          Console.WriteLine(opts.GetUsage());
        }
      }
      catch (DockerException ex)
      {
        logger.Exit($"Exited with exit code ({ex.ExitCode}: docker {ex.Message})");
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

    private void ShowApp(DockerApps apps, string appName)
    {
      var toRun = apps.GetApps(appName).ToArray();

      if (!CheckApps(toRun)) return;

      var app = toRun[0];
      var container = app.Container;

      // DockerContainer
      Console.Out.WriteLine("DockerContainer: ");

      var status = Docker.GetContainerStatus(container.Name);
      Console.Out.Write(string.Format($"\t{container.Name} ({status})\n"));

      // DockerContainer Path
      Console.Out.WriteLine("Path: ");
      Console.Out.Write(string.Format($"\t{container.ContainerPath}\n"));

      // Mounts
      Console.Out.WriteLine("Mounts: ");
      foreach (var mount in container.Config.Mounts)
      {
        Console.Out.Write(string.Format($"\t{mount.Type.ToUpper()}: {mount.Host} => {mount.Container}\n"));
      }
    }
  }
}