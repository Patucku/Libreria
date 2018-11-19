using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Verificar()
        {

            return View();
        }

        public ActionResult Salas()
        {

            return View();
        }

        public ActionResult verSala()
        {

            return View();
        }

        public ActionResult Materias()
        {

            return View();
        }

        public ActionResult verMateria()
        {

            return View();
        }

        public ActionResult Libros()
        {

            return View();
        }

        public ActionResult verLibro()
        {

            return View();
        }

        public ActionResult Reportes()
        {

            return View();
        }

        public ActionResult Pedidos()
        {

            return View();
        }
    }
}
