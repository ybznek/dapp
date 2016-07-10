namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    public string GetHostname(string name)
      => $"{name.ToLower().Replace('_', '.')}.{Prefix.ToLower().Replace('_', '.')}";

    public string GetContainerName(string name)
      => $"{Prefix}_{name}";

    public string GetImageName(string name)
      => $"{Prefix.ToLower()}/{name}";
  }
}