using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgentRalph;
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
          var model = "";
            return View();
        }

      public JsonResult Data()
      {
        var code = "2 + 3 * (5-7)";
        var code2 = "2+3 * (5+7)";

        var expr = AstMatchHelper.ParseToExpression(code);
        var expr2 = AstMatchHelper.ParseToExpression(code2);

        var matchResult = AstMatchHelper.MatchesWithState(expr, expr2);

        var r = new
        {
	  name="root",
	  // mark the divergent node red, then assemble the two into a big tree.
          children = new[] { new{name="left", children=new[]{matchResult.FailNodeLeft.ToJson()}},
                             new{name="right", children=new[]{matchResult.FailNodeRight.ToJson()}},
                             new{name="root", children=new[]{matchResult.Root.ToJson()}}
          }
        };
        return Json(r, JsonRequestBehavior.AllowGet);
      }

    }
}
