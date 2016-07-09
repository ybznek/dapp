namespace Docker_app.Dapp.Configuration
{

    public class Mount : IMount
    {
      public string Host => host;
      public string Container => container;
      public string Type => type;

      public string host;
      public string container;
      public string type;
    }
}