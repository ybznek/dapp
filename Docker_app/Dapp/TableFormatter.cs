using System.Collections.Generic;
using System.IO;
using System.Text;
using StringTuple = System.Tuple<string, string>;
using DataEnumerable = System.Collections.Generic.IEnumerable<System.Tuple<string, string>>;

namespace Docker_app.Dapp
{
  public static class TableFormatter
  {
    private const string DefaultFormat = "\t{0}\t{1}\n";

    public static void Format(StringBuilder b, DataEnumerable enumerable, string formatString = DefaultFormat)
    {
      int max;
      List<StringTuple> list;
      PrepareData(enumerable, out max, out list);

      // Get aligned output
      foreach (var l in list)
      {
        var text = l.Item1.PadRight(max);
        var description = l.Item2;
        b.AppendFormat(formatString, text, description);
      }
    }

    public static void Format(TextWriter w, DataEnumerable enumerable, string formatString = DefaultFormat)
    {
      int max;
      List<StringTuple> list;
      PrepareData(enumerable, out max, out list);

      // Get aligned output
      foreach (var l in list)
      {
        var text = l.Item1.PadRight(max);
        var description = l.Item2;
        w.Write(formatString, text, description);
      }
    }

    private static void PrepareData(DataEnumerable enumerable, out int max, out List<StringTuple> list)
    {
      list = new List<StringTuple>();
      max = 0;

      // Fill list & get max
      foreach (var v in enumerable)
      {
        list.Add(v);
        var l = v.Item1.Length;
        if (l > max)
        {
          max = l;
        }
      }
    }
  }
}