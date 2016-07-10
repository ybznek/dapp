using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;

namespace Docker_app.Dapp.Docker_runner
{
  public class ParamsBuilder
  {
    private readonly bool _escape;

    public ParamsBuilder(bool escape=true)
    {
      _escape = escape;
    }

    private readonly List<string> _args = new List<string>();

    public IEnumerable<string> Params => _args;

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
      _args.Add(param);
    }

    public void Append(string param)
    {
      if (_escape)
      {
        var p = param.Replace("\"", "\\\"");
        _args.Add($"\"{p}\"");
      }
      else
      {
        _args.Add(param);
      }
    }

    public static implicit operator string(ParamsBuilder p)
    {
      return p.ToString();
    }


    public override string ToString() => string.Join(" ", _args);
  }
}