using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CommandLine;
using Docker_app.Dapp.Configuration;
using StringTuple = System.Tuple<string, string>;

// if you want text formatting helpers (recommended)

namespace Docker_app.Dapp.Commandline
{
  class Options
  {
    [Option("l", "length", DefaultValue = -1, HelpText = "The maximum number of bytes to process.")]
    public int MaximumLength { get; set; }

    [Option("v", "verbose", HelpText = "Print details during execution.")]
    public bool Verbose { get; set; }

    [Option("r", "Run", HelpText = "DockerApp")]
    public string App { get; set; }

    [Option("h", "help", DefaultValue = false, HelpText = "Print help")]
    public bool Help { get; set; }

    [Option("l", "list", HelpText = "Print list of apps")]
    public bool List { get; set; }

    [Option("d", "details", HelpText = "Print details of app")]
    public string Details { get; set; }

    [Option("b", "rebuild", HelpText = "Force rebuild container")]
    public bool Rebuild { get; set; }

    [Option("c", "recreate", HelpText = "Remove and build container again")]
    public bool Recreate { get; set; }

    [Option("o", "optimize", HelpText = "Optimize dockerfile - remove layers")]
    public bool Optimize { get; set; }

    public RunOptions RunOpts
    {
      get
      {
        var runOpts = RunOptions.None;
        if (Rebuild)
        {
          runOpts |= RunOptions.Rebuild;
        }

        if (Recreate)
        {
          runOpts |= RunOptions.Recreate;
        }

        if (Optimize)
        {
          runOpts |= RunOptions.Optimize;
        }

        return runOpts;
      }
    }

    [HelpOption]
    public string GetUsage()
    {
      var usage = new StringBuilder();

      // Usage
      usage.Append("Usage:\n");
      usage.AppendFormat("\t{0} [OPTIONS]\n\n", System.AppDomain.CurrentDomain.FriendlyName);

      // Options
      usage.Append("Options:\n");
      TableFormatter.Format(usage, GetAttributes());

      return usage.ToString();
    }


    private IEnumerable<Tuple<string, string>> GetAttributes()
    {
      // Properties
      var props = GetType().GetProperties();
      foreach (var prop in props)
      {
        // Attributes
        var attrs = prop.GetCustomAttributes<OptionAttribute>(false);

        foreach (var attr in attrs)
        {
          var text = (prop.PropertyType == typeof(bool))
              ? $"-{attr.ShortName}, --{attr.LongName}"
              : $"-{attr.ShortName}, --{attr.LongName}={attr.LongName.ToUpper()}"
            ;
          var description = attr.HelpText;
          yield return new StringTuple(text, description);
        }
      }
    }
  }
}