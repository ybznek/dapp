using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace Docker_app.Dapp.Configuration
{
  public class ConfigData
  {
    public Dictionary<string, string> exec;
    public Mount[] mount;
    public Dictionary<string, string> vars;
  }

  public class DockerConfig
  {
    private readonly ConfigData _configData;

    public Dictionary<string, string> Vars => _configData.vars;

    public IEnumerable<IMount> Mounts => _configData.mount;

    public DockerConfig(string filename)
    {
      var content = File.ReadAllText(filename);
      _configData = JsonConvert.DeserializeObject<ConfigData>(content);
      if (_configData.exec == null)
      {
        _configData.exec = new Dictionary<string, string>();
      }
    }

    public string GetDefaultExec()
    {
      return _configData.exec["default"];
    }

    public ReadOnlyDictionary<string, string> GetExecList()
    {
      return new ReadOnlyDictionary<string, string>(_configData.exec);
    }

    public string GetExecRunnable(string name)
    {
      return _configData.exec[name];
    }
  }
}