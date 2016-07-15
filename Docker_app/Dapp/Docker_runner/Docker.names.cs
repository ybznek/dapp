namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {
    public string GetHostname(string name)
      => $"{name.ToLower().Replace('_', '.')}.{_config.HostnameSuffix}";

    public string GetContainerName(string name)
      => $"{_config.ContainerPrefix}_{name}";

    public string GetImageName(string name)
      => $"{_config.ImagePrefix}/{name.ToLower()}";
  }
}