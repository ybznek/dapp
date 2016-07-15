using System.Runtime.Serialization;

namespace Docker_app.Dapp.Configuration
{
  [DataContract]
  public class DesktopConfig
  {
    [DataMember(Name = "icon")] public string icon;

    [DataMember(Name = "name")] public string name;

    [DataMember(Name = "comment")] public string comment;

    [DataMember(Name = "terminal")] public string terminal;
  }
}