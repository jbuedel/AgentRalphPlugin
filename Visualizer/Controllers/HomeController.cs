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
        string code = "2 + 3";
        var expr = AstMatchHelper.ParseToExpression(code);
        return Json(expr.ToJson(), JsonRequestBehavior.AllowGet);
      }

    }
}
