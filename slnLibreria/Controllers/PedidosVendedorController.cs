using Microsoft.AspNet.Identity;
using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    [Authorize(Roles = "Administrador, Vendedor")]
    public class PedidosVendedorController : Controller
    {
        // GET: PedidosVendedor
        public ActionResult Index()
        {
            ModelState.Clear();
            PedidosView objPedido = CargarIndex();
            return View(objPedido);
        }

        [HttpPost]
        public ActionResult Index(PedidosView objBuscar)
        {
            if (string.IsNullOrEmpty(objBuscar.clienteID.ToString()))
            {
                PedidosView objPedido = CargarIndex();
                return View(objPedido);
            }
            else
            {
                PedidosView objPedido = CargarIndex(objBuscar.clienteID);
                return View(objPedido);
            }
        }

        private static PedidosView CargarIndex()
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                PedidosView objPedidos = new PedidosView();
                objPedidos.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 2).ToList();
                return objPedidos;
            }
        }

        private static PedidosView CargarIndex(int clienteId)
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                PedidosView objPedidos = new PedidosView();
                objPedidos.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 2 && n.clienteID == clienteId).ToList();
                if (clienteId == 0)
                {
                    objPedidos.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 2).ToList();
                }
                return objPedidos;
            }
        }
        
        public ActionResult despacharPedido(int? id)
        {
            if (id == 0)
            {
                ViewBag.ErrorDespachar = "Seleccione un pedido para despachar";
                return View("Index", CargarIndex());
            }
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                Pedido despacharPedido = db.Pedido.Where(n => n.pedidoID == id).FirstOrDefault();
                if (despacharPedido == null || despacharPedido.estadopedidoID != 2)
                {
                    ViewBag.ErrorDespachar = "El pedido no puede ser despachado";
                    return View("Index", CargarIndex());
                }
                else
                {
                    try
                    {
                        string usuario = User.Identity.GetUserId();
                        Vendedor vendedor = db.Vendedor.Where(n => n.aspUserID == usuario && n.vendedorEstado == true).FirstOrDefault();
                        despacharPedido.estadopedidoID = 3;
                        despacharPedido.vendedorID = vendedor.aspUserID;
                        db.Entry(despacharPedido).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Despachado = "El pedido: " + despacharPedido.pedidoID + " fue despachado";
                        return View("Index", CargarIndex());
                    }
                    catch(Exception ex)
                    {
                        ViewBag.ErrorDespachar = "Erro al momento de despachar el pedido\nError: " +ex.Message ;
                        return View("Index", CargarIndex());
                    }
                }
            }
        }
    }
}