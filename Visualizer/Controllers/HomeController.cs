using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AgentRalph;
using ICSharpCode.NRefactory.Ast;
using Microsoft.Ajax.Utilities;

namespace Visualizer.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
//          var file = System.IO.Directory.EnumerateFiles(@"C:\Users\jbuedel\Projects\agentralphplugin\bin\", "*.json").First();
//          var json = System.IO.File.ReadAllText(file);
            return View();
        }

      private JNode build(JNode tree, int targetid, JNode addition)
      {
        if (tree.id == targetid)
          return new JNode {name = "divergence", children = new[] {tree, addition}, id=0};

        var children = tree.children.Select(v => build(v, targetid, addition)).ToList();
        return new JNode{name=tree.name, data=tree.data, children=children, id=0};
      }

      public JsonResult Data()
      {
        var code = "2 + 3 * (5-7)";
        var code2 = "2+3 * (5+7)";

        var expr = AstMatchHelper.ParseToExpression(code);
        var expr2 = AstMatchHelper.ParseToExpression(code2);

        var matchResult = AstMatchHelper.MatchesWithState(expr, expr2);
        var r = build(matchResult.Root.ToJson(), 
          matchResult.FailNodeLeft.ToJson().id,
          matchResult.FailNodeRight.ToJson());
        return Json(r, JsonRequestBehavior.AllowGet);
      }
    }
}

