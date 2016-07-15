using System.Linq;
using Docker_app.Dapp.Configuration;

namespace Docker_app.Dapp.Structures
{
  public class DockerApp
  {
    public DockerContainer Container { get; private set; }
    public string Name { get; private set; }

    public DesktopConfig Desktop =>
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