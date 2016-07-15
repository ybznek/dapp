using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Docker_app.Dapp.Configuration;
using Docker_app.Dapp.Structures;

namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    private DappConfig _config;
    private IDockerLogger Logger { get; }

    public Docker(DappConfig config, IDockerLogger logger)
    {
      Logger = logger;
      _config = config;
    }

    protected ParamsBuilder Params(bool escape = true) => new ParamsBuilder(escape);

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

    public void Build(string imageName, string path, Dictionary<string, string> dictionary = null,
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
        Logger.Dockerfile(line, important: true);
      }

      Environment.CurrentDirectory = path;
      var args = Params()
                 | "build"
                 | "-t"
                 | imageName
                 | "-";

      var process = Run(args);
      using (var inp = process.StandardInput)
      {
        inp.WriteLine(dockerfile);
      }

      foreach (var line in process.ReadOutputLines())
      {
        Logger.Output(line, true);
      }
      process.WaitForExit();
      if (process.ExitCode != 0)
      {
        throw new DockerException(args, process.ExitCode);
      }
    }

    public bool RunApp(string path, IDappApp dapApp, ContainerConfig config,
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

      if (options.HasFlag(RunOptions.Recreate) || (status == ContainerStatus.Stopped))
      {
        RemoveContainer(containerName);
        status = ContainerStatus.NotExists;
      }

      if (options.HasFlag(RunOptions.Rebuild) || (status == ContainerStatus.NotExists))
      {
        Build(imageName, path, config.Vars, options.HasFlag(RunOptions.Optimize));
        status = ContainerStatus.Running;

        RunContainer(containerName, config, name, imageName);
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
        ExecProgram(containerName, dapApp);
      }


      return true;
    }

    protected void AppendUserParam(ParamsBuilder b, string user)
    {
      if (string.IsNullOrEmpty(user))
      {
        var uid = GetUseId("-u");
        var gid = GetUseId("-g");
        b = b | "-u" | $"{uid}:{gid}";
      }
      else if (user == "root")
      {
        b = b | "-u" | "0:0";
      }
    }

    protected void ExecProgram(string containerName, IDappApp dapApp)
    {
      var display = Environment.GetEnvironmentVariable("DISPLAY");


      var args = Params(!dapApp.ExecProcess) | "exec";

      dapApp.Flags?.Aggregate(args, (current, flag) => current | flag);

      AppendUserParam(args, dapApp.User);

      args = args | containerName
             | "sh" | "-c" | (
               Params()
               + $"DISPLAY={display}"
               + $"{dapApp.Cmd}"
             );

      if (dapApp.ExecProcess)
      {
        Exec(args);
      }
      else
      {
        Run(
          param: args,
          stdin: false,
          stdout: false,
          stderr: false
        );
      }
    }


    public void RunDockerApp(DockerApp app, RunOptions options)
    {
      var c = app.Container;
      var path = c.ContainerPath;
      var appRunnable = c.Config.GetExecRunnable(app.Name);
      RunApp(path, appRunnable, c.Config, options);
    }
  }
}