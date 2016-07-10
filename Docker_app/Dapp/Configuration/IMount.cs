namespace Docker_app.Dapp.Configuration
{
  public interface IMount
     {
       string Host { get; }
       string Container { get; }
       string Mode { get; }
     }
}