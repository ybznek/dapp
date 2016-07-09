
namespace Docker_app.Dapp.Configuration
{
  public class DockerApp
  {
    public DockerContainer Container { get; private set; }
    public string Name { get; private set; }

    public DockerApp(DockerContainer dockerContainer, string name)
    {
      Container = dockerContainer;
      Name = name;
    }
  }
}