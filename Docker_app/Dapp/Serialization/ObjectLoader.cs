using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;

namespace Docker_app.Dapp.Serialization
{
  public class ObjectLoader
  {
    public class UnknownEntityException : ApplicationException
    {
      public UnknownEntityException(string entity) : base("Unknown entity" + entity)
      {
      }
    }

    public static void Store<T>(string filename, T obj)
    {
      var dir = Path.GetDirectoryName(filename);
      Debug.Assert(dir != null);
      Directory.CreateDirectory(dir);

      using (var fs = File.Open(filename, FileMode.Create, FileAccess.Write))
      {
        var settings = new XmlWriterSettings {Indent = true};
        using (var w = XmlWriter.Create(fs, settings))
        {
          var ser = new DataContractSerializer(typeof(T));
          ser.WriteObject(w, obj);
        }
      }
    }

    public static T LoadFromXmlFile<T>(string filename, IReadOnlyDictionary<string, string> entities) where T : class
    {
      var text = File.ReadAllText(filename);
      var updated = ReplaceEntities(text, entities);
      var bytes = Encoding.UTF8.GetBytes(updated);
      using (var ms = new MemoryStream(bytes))
      {
        var s = new DataContractSerializer(typeof(T));
        return (T) s.ReadObject(ms);
      }
    }


    private static string ReplaceEntities(string text, IReadOnlyDictionary<string, string> entities)
    {
      return Regex.Replace(text, @"&(\w*);", match =>
      {
        var entity = match.Groups[1].Value;
        string result;
        return !entities.TryGetValue(entity, out result) ? entity : result;
      });
    }

    public static T LoadFromJsonFile<T>(string filename, object dict) where T : class
    {
      var content = File.ReadAllText(filename);
      return JsonConvert.DeserializeObject<T>(content);
    }
  }
}