using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WriteQueue.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string info)
        {
            var cc = ConfigurationManager.AppSettings["connectionString"];
            QueueHandler.Instance.CrearCola(cc, "incidencias", 1024, 86400);

            var d = new Dictionary<string, string>()
            {
                {"Incidencia", info},
                {"fecha", DateTime.Now.ToLongTimeString()}
            };

            QueueHandler.Instance.Enviar(cc, "incidencias", d, "Incidencia");

            return RedirectToAction("Index");
        }
    }
}