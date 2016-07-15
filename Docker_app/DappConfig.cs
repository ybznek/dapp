using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Docker_app.Dapp.Serialization;

namespace Docker_app
{
  [DataContract]
  public class Constant
  {
    public Constant(string name, string value)
    {
      this.name = name;
      this.value = value;
    }

    [DataMember] public string name;
    [DataMember] public string value;
  }

  [DataContract]
  public class ConfigData
  {
    [DataMember] public string AppsDir;

    [DataMember] public string ContainerPrefix;

    [DataMember] public string ImagePrefix;
    [DataMember] public string DesktopPrefix;
    [DataMember] public string HostnameSuffix;

    public Dictionary<string, string> Consts = new Dictionary<string, string>();

    [DataMember(Name = "Constants")]
    public IEnumerable<Constant> C
    {
      get { return Consts.Select(kv => new Constant(kv.Key, kv.Value)); }
      set
      {
        Consts = new Dictionary<string, string>();
        foreach (var kv in value)
        {
          Consts.Add(kv.name, kv.value);
        }
      }
    }
  }

  public class DappConfig
  {
    public IReadOnlyDictionary<string, string> Entities => _entities;
    private readonly Dictionary<string, string> _entities = new Dictionary<string, string>();

    private string UserDir => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private ConfigData _configData = null;

    private string _configPath;

    public string AppsDir => _configData.AppsDir;

    public string ContainerPrefix => _configData.ContainerPrefix;
    public string ImagePrefix => _configData.ImagePrefix;
    public string DesktopPrefix => _configData.DesktopPrefix;
    public string HostnameSuffix => _configData.HostnameSuffix;

    public DappConfig(string configPath)
    {
      _configPath = configPath;
    }

    public IReadOnlyDictionary<string, string> ConstList
      => _configData.Consts;

    public void Load()
    {
      var configFile = Path.Combine(_configPath, "settings.xml");

      var d = new Dictionary<string, string>
      {
        {"configDir", "/home/z"},
      };
      try
      {
        _configData = ObjectLoader.LoadFromXmlFile<ConfigData>(configFile, d);

        foreach (var p in _configData.Consts)
        {
          Console.WriteLine(p.Key + ":" + p.Value);
        }
      }
      catch (IOException ex)
      {
        _configData = new ConfigData
        {
          AppsDir = Path.Combine(UserDir, "Dapps"),
          Consts = new Dictionary<string, string>(),
          ContainerPrefix = "Dapps",
          ImagePrefix = "dapp.docker.app",
          DesktopPrefix = "Dapp",
          HostnameSuffix ="dapp",
        };
        ObjectLoader.Store(configFile, _configData);
      }

      var c = _configData.Consts;
      c.Add("dappConfigPath", _configPath);
      c.Add("userDir", UserDir);
      c.Add("userName", Environment.UserName);
      c.Add("appsDir", _configData.AppsDir);
      c.Add("imagePrefix", _configData.ImagePrefix);
      c.Add("containerPrefix", _configData.ContainerPrefix);
      c.Add("hostnameSuffix", _configData.HostnameSuffix);
    }
  }
}