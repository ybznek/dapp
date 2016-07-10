namespace Docker_app.Dapp.Docker_runner
{
  public partial class Docker
  {

    public string GetVersion() => RunNowOutput(Params()|"--version");
  }
}