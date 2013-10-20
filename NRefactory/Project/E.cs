using System.Collections.Generic;
using System.Dynamic;
using System.Web.Routing;

public static class E {
  public static dynamic ToExpando(this object obj)
  {
    IDictionary<string, object> d = new RouteValueDictionary(obj);
    IDictionary<string, object> x = new ExpandoObject();
    foreach (var m in d)
      x.Add(m);
    return x;
  }
}