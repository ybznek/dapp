using System;

namespace Docker_app.Dapp.Configuration
{
    [Flags]
    public enum RunOptions : byte
    {
      None = 0,
      NeedKill = 1 << 0,
      Rebuild = (1 << 1) | NeedKill,
      Recreate = (1 << 2) | NeedKill,
      Optimize = 1 << 3,
    }
}