using System.Runtime.CompilerServices;
using System.Text;

namespace Docker_app.Dapp.Docker_runner
{
  public class ParamsBuilder
  {
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ParamsBuilder operator |(ParamsBuilder b, string param)
    {
      b.Append(param);
      return b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ParamsBuilder operator +(ParamsBuilder b, string param)
    {
      b.AppendNotEscape(param);
      return b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ParamsBuilder operator |(ParamsBuilder a, ParamsBuilder b)
    {
      a.Append(b.ToString());
      return a;
    }

    public void AppendNotEscape(string param)
    {
      if (_stringBuilder.Length != 0)
      {
        _stringBuilder.Append(' ');
      }
      _stringBuilder.Append(param);
    }

    public void Append(string param)
    {
      if (_stringBuilder.Length != 0)
      {
        _stringBuilder.Append(' ');
      }

      var p = param.Replace("\"", "\\\"");
      _stringBuilder.Append($"\"{p}\"");
    }

    public override string ToString() => _stringBuilder.ToString();
  }
}