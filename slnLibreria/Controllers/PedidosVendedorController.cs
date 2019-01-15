using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class PedidosVendedorController : Controller
    {
        // GET: PedidosVendedor
        public ActionResult Index(PedidosView objPedido)
        {
            
            var usuario = Session["ClienteIngresado"] as Cliente;
            return View();
        }
    }
}