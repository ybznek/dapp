using System.Runtime.CompilerServices;

namespace Docker_app
{
  struct Name<T>
  {
    private readonly string _str;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Name(string str)
    {
      _str = str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator string(Name<T> name) => name._str;
  }
}