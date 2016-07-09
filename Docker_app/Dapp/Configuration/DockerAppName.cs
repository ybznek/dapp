using System.Diagnostics.Contracts;

namespace Docker_app.Dapp.Configuration
{
  public class DockerAppName
  {
    public string Container { get; private set; }
    public string App { get; private set; }

    public DockerAppName(string appName)
    {
      Contract.Requires(appName != null);
      var delimiter = new[] {'/'};
      var output = appName.Split(delimiter, 2);
      if (output.Length == 2)
      {
        Container = output[0];
        App = output[1];
      }
      else
      {
        Container = null;
        App = appName;
      }
    }

    public bool Equals(string appName) => App.Equals(appName);

    public bool Equals(string containerName, string appName)
    {
      if (Container == null)
      {
        return App.Equals(appName);
      }
      else
      {
        return App.Equals(appName) && Container.Equals(containerName);
      }
    }
  }
}