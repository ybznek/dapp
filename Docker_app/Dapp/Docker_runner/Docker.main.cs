using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    public string Prefix { get; protected set; }

    private IDockerLogger Logger { get; }

    public Docker(string prefix, IDockerLogger logger)
    {
      Prefix = prefix;
      Logger = logger;
    }

    protected ParamsBuilder Params() => new ParamsBuilder();

    public string LoadDockerfile(string filepath, Dictionary<string, string> dictionary)
    {
      var contents = File.ReadAllText(filepath);

      if (dictionary == null)
      {
        return contents;
      }
      else
      {
        var output = new StringBuilder(contents);
        foreach (var replace in dictionary)
        {
          var from = $"<<{replace.Key}>>";
          var to = replace.Value;
          output.Replace(from, to);
        }
        return output.ToString();
      }
    }

    public string GetUseId(string val)
    {
      var processStartInfo = new ProcessStartInfo
      {
        FileName = "id",
        Arguments = val,
        RedirectStandardOutput = true,
        UseShellExecute = false
      };
      var process = Process.Start(processStartInfo);
      var output = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      return output.TrimEnd();
    }

    public bool Build(string imageName, string path, Dictionary<string, string> dictionary = null,
      bool optimize = false)
    {
      if (optimize)
      {
        throw new NotImplementedException();
      }

      var filepath = $@"{path}/Dockerfile";
      var dockerfile = LoadDockerfile(filepath, dictionary);

      foreach (var line in dockerfile.Split('\n'))
      {
        Logger.Dockerfile(line);
      }

      Environment.CurrentDirectory = path;
      var args = Params()
                 | "build"
                 | "-t"
                 | imageName;

      var process = Run(args);
      using (var inp = process.StandardInput)
      {
        inp.WriteLine(dockerfile);
      }

      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line);
      }
      process.WaitForExit();
      return process.ExitCode == 0;
    }

    public bool RunApp(string path, string execName, Dictionary<string, string> dictionary = null,
      RunOptions options = RunOptions.None)
    {
      var name = new DirectoryInfo(path).Name;
      var containerName = GetContainerName(name);
      var imageName = GetImageName(name);
      var status = GetContainerStatus(containerName);

      if (options.HasFlag(RunOptions.NeedKill) && (status == ContainerStatus.Running))
      {
        KillContainer(containerName);
        status = ContainerStatus.Stopped;
      }

      if (options.HasFlag(RunOptions.Recreate) && (status == ContainerStatus.Stopped))
      {
        RemoveContainer(containerName);
        status = ContainerStatus.NotExists;
      }

      if (status == ContainerStatus.NotExists)
      {
        Build(imageName, path, dictionary, options.HasFlag(RunOptions.Optimize));
        status = ContainerStatus.Running;

        RunContainer(containerName, name, imageName);
        status = ContainerStatus.Running;
      }
      if (status == ContainerStatus.Stopped)
      {
        var args = Params()
                   | "start"
                   | containerName;

        RunNow(args);
        status = ContainerStatus.Running;
      }

      if (status == ContainerStatus.Running)
      {
        ExecProgram(containerName, execName);
      }


      return true;
    }


    protected void ExecProgram(string containerName, string execName)
    {
      var uid = GetUseId("-u");
      var gid = GetUseId("-g");
      var display = Environment.GetEnvironmentVariable("DISPLAY");

      var args = Params()
                 | "exec" | "-t" | "-u"
                 | $"{uid}:{gid}"
                 | containerName
                 | "sh" | "-c" | (
                   Params()
                   + $"DISPLAY={display}"
                   | $"{execName}"
                 );

      Run(
        param: args,
        stdin: false,
        stdout: false,
        stderr: false
      );
    }


    public void RunDockerApp(DockerApp app, RunOptions options)
    {
      var c = app.Container;
      var path = c.ContainerPath;
      var config = c.Config.Vars;
      var appRunnable = c.Config.GetExecRunnable(app.Name);
      RunApp(path, appRunnable, config, options);
    }
  }
}