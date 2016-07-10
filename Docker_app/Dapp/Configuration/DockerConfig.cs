using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Docker_app.Dapp.Configuration
{
  public class ConfigData
  {
    public Dictionary<string, Exec> exec;
    public Mount[] mount;
    public Dictionary<string, string> vars;
    public string[] flags;

  }

  public class DockerConfig
  {
    private readonly ConfigData _configData;

    public Dictionary<string, string> Vars => _configData.vars;

    public IEnumerable<IMount> Mounts => _configData.mount;

    public IEnumerable<string> Flags => _configData.flags;



    public DockerConfig(string filename)

    {
      var content = File.ReadAllText(filename);
      _configData = JsonConvert.DeserializeObject<ConfigData>(content);
      if (_configData.exec == null)
      {
        _configData.exec = new Dictionary<string, Exec>();
      }
    }

    public IEnumerable<KeyValuePair<string, IExec>> GetExecList()
    {
      return _configData.exec.Select(
        kv => new KeyValuePair<string, IExec>(kv.Key, kv.Value)
      );
    }

    public IExec GetExecRunnable(string name)
    {
      return _configData.exec[name];
    }
  }
}