using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Docker_app.Dapp.Serialization;

namespace Docker_app.Dapp.Configuration
{
  public static class SerializationNamespace
  {
    public const string Config = "http://schemas.datacontract.org/2004/07/Docker_app.Container";
  }

  [CollectionDataContract(ItemName = "flag")]
  public class FlagsList : List<string>
  {
  }

  [CollectionDataContract(ItemName = "mount")]
  public class MountsList : List<Mount>
  {
  }

  [CollectionDataContract(ItemName = "var", KeyName = "name", ValueName = "value")]
  public class VarsDict : Dictionary<string, string>
  {
  }

  [CollectionDataContract(ItemName = "app", KeyName = "name", ValueName = "config")]
  public class AppsDict : Dictionary<string, DappApp>
  {
  }


  [DataContract]
  public class AppConfigData
  {
    [DataMember(Name = "apps")] public AppsDict Apps = new AppsDict();

    [DataMember(Name = "mounts")] public MountsList mount = new MountsList();

    [DataMember(Name = "variables")] public VarsDict Vars = new VarsDict();

    [DataMember(Name = "flags")] public FlagsList Flags = new FlagsList();
  }

  public class ContainerConfig
  {
    private readonly AppConfigData _appConfigData;

    public Dictionary<string, string> Vars => _appConfigData.Vars;

    public IEnumerable<IMount> Mounts => _appConfigData.mount;

    public IEnumerable<string> Flags => _appConfigData.Flags;

    public ContainerConfig(string filename, IReadOnlyDictionary<string, string> constants)
    {
      var dict = new Dictionary<string, string>();
      if (filename.EndsWith(".json"))
      {
        _appConfigData = ObjectLoader.LoadFromJsonFile<AppConfigData>(filename, dict);
        if (_appConfigData.Apps == null)
        {
          _appConfigData.Apps = new AppsDict();
        }
      }
      else
      {
        _appConfigData = ObjectLoader.LoadFromXmlFile<AppConfigData>(filename, dict);
      }
    }

    public static void MakeTemplate(string filename)
    {
      var data = new AppConfigData();
      data.Flags.Add("flag");
      data.Vars.Add("key", "value");
      data.Apps.Add("my", new DappApp("filename"));
      ObjectLoader.Store(filename, data);
    }

    public IEnumerable<KeyValuePair<string, IDappApp>> GetExecList()
    {
      return _appConfigData.Apps.Select(
        kv => new KeyValuePair<string, IDappApp>(kv.Key, kv.Value)
      );
    }

    public IDappApp GetExecRunnable(string name)
    {
      return _appConfigData.Apps[name];
    }
  }
}