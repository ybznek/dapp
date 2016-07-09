namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    protected string GetHostname(string name)
      => $"{name.ToLower().Replace('_', '.')}.{Prefix.ToLower().Replace('_', '.')}";

    protected string GetContainerName(string name)
      => $"{Prefix}_{name}";

    protected string GetImageName(string name)
      => $"{Prefix.ToLower()}/{name}";
  }
}