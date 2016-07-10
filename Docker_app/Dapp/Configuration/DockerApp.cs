
using System.Collections.Generic;
using System.Linq;

namespace Docker_app.Dapp.Configuration
{
  public class DockerApp
  {
    public DockerContainer Container { get; private set; }
    public string Name { get; private set; }

    public Dictionary<string, string> Desktop =>
         (from e in Container.Config.GetExecList()
           where e.Key.Equals(Name)
           select e.Value.Desktop
         ).FirstOrDefault();

    public DockerApp(DockerContainer dockerContainer, string name)
    {
      Container = dockerContainer;
      Name = name;
    }
  }
}