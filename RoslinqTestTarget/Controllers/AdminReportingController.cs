using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoslinqTestTarget.Controllers
{
    using System.Web.Mvc;
    using Models;

    public class AdminReportingController : AdminController
    {
        public ActionResult Baz(SerializableModel model)
        {
            return View();
        }
    }
}