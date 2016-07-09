using System;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Commandline
{
  public class DockerLogger : IDockerLogger
  {
    private bool Verbose { get; }

    public DockerLogger(bool verbose, bool important = false)
    {
      Verbose = verbose;
    }

    public void Command(string cmd, bool important = false)
    {
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      Console.Write($"[cmd]");
      Console.Write($" docker {cmd}\n");
      Console.ResetColor();
    }

    public void Output(string msg, bool important = false)
    {
      Console.ForegroundColor = ConsoleColor.DarkBlue;
      Console.Write($"[output]");
      Console.Write($" {msg}\n");
      Console.ResetColor();
    }

    public void Exit(string msg, bool important = false)
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.Write($"[output]");
      Console.Write($" {msg}\n");
      Console.ResetColor();
    }

    public void Dockerfile(string msg, bool important = false)
    {
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write($"[dockerfile]");
      Console.ResetColor();
      Console.Write($" {msg}\n");
    }
  }
}