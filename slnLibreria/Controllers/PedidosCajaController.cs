using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    [Authorize(Roles = "Administrador, Caja")]
    public class PedidosCajaController : Controller
    {
        // GET: PedidosCaja
        public ActionResult Index()
        {
            PedidosView objPedido = CargarIndex();
            return View(objPedido);
        }

        private static PedidosView CargarIndex()
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                PedidosView objPedidos = new PedidosView();
                objPedidos.pedidosPorLiquidar = db.View_Listar_Pedidos_Por_Lq_Clientes.ToList();
                return objPedidos;
            }
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

        private static PedidosView CargarIndex(int clienteId)
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                PedidosView objPedidos = new PedidosView();

                objPedidos.pedidosPorLiquidar = db.View_Listar_Pedidos_Por_Lq_Clientes.Where(n=> n.clienteID == clienteId).ToList();
                if (clienteId == 0)
                {
                    objPedidos.pedidosPorLiquidar = db.View_Listar_Pedidos_Por_Lq_Clientes.ToList();
                }
                return objPedidos;
            }
        }

        //Cargar todos los pedidiso por liquidar del usuario y los pedidos por despachar
        public ActionResult liquidarPedidos(int? id)
        {
            if(id == null)
            {
                ViewBag.ErrorFinalizar = "Seleccione un cliente para liquidar sus compras";
                return View("Index", CargarIndex());
            }
            else
            {
                PedidosView objPedidos = new PedidosView();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objPedidos.pedidosPorLiquidar = db.View_Listar_Pedidos_Por_Lq_Clientes.Where(n=> n.clienteID == id).ToList();
                    if (objPedidos.pedidosPorLiquidar == null)
                    {
                        ViewBag.ErrorFinalizar = "No existen pedidos por liquidar";
                        return View("Index", CargarIndex());
                    }
                    else
                    {
                        objPedidos.pedidosClientePorEstado = db.View_Listar_Pedidos_ResumenCliente.Where(n => n.estadopedidoID == 2 && n.clienteID == id).ToList();     
                        objPedidos.DetallePedidosFinCan = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 2 && n.clienteID == id).ToList();
                        objPedidos.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 3 && n.clienteID == id).ToList();
                        return View(objPedidos);
                    }
                }
            }
        }
        
        //DepsuesDeVerificar el detalle se procede a liquidar el pedido
        public ActionResult liquidarPedidosConfirmado(int? usuario)
        {
            if (usuario == null)
            {
                ViewBag.ErrorFinalizar = "Seleccione un cliente para liquidar sus compras";
                return View("Index", CargarIndex());
            }
            else
            {
                PedidosView objPedidosLiquidar = new PedidosView();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objPedidosLiquidar.pedidosCliente = db.Pedido.Where(n => n.estadopedidoID == 3 && n.clienteID == usuario).ToList();
                    if (objPedidosLiquidar.pedidosCliente == null)
                    {
                        ViewBag.ErrorFinalizar = "No existen pedidos por liquidar";
                        return View("Index", CargarIndex());
                    }
                    else
                    {
                        try
                        {
                            foreach (var pedidoLiquido in objPedidosLiquidar.pedidosCliente)
                            {
                                pedidoLiquido.estadopedidoID = 4;
                                pedidoLiquido.fechaFinPedido = DateTime.Now;
                                db.Entry(pedidoLiquido).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                        catch(Exception ex)
                        {
                            ViewBag.ErrorFinalizar = "No se pueden liquidar los pedidos del cliente";
                            return View("Index", CargarIndex());
                        }
                        
                        ViewBag.Finalizado = "Los pedidos del cliente fueron liquidados";
                        return View("Index", CargarIndex());
                    }
                }
            }
        }

        public ActionResult cancelarPedido(int? id)
        {
            int codigoUsuario=0;
            if (id == 0 || id == null)
            {
                ViewBag.ErrorEliminar = "No existen libros en la búsqueda seleccionada";
                return View("Index", CargarIndex());
            }
            else
            {
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        Pedido pedidoCancelar = db.Pedido.Where(n => n.pedidoID == id).FirstOrDefault();
                        codigoUsuario = pedidoCancelar.clienteID;
                        LibroSala librosalaActualizar = db.LibroSala.Where(n => n.librosalaID == pedidoCancelar.librosalaID).FirstOrDefault();
                        if (pedidoCancelar.estadopedidoID > 1 && pedidoCancelar.estadopedidoID < 4)
                        {
                            librosalaActualizar.cantidadLibroSala = librosalaActualizar.cantidadLibroSala + pedidoCancelar.cantidadPedido;
                            pedidoCancelar.estadopedidoID = 5;
                            pedidoCancelar.fechaFinPedido = DateTime.Now;
                            db.Entry(pedidoCancelar).State = EntityState.Modified;
                            db.Entry(librosalaActualizar).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.ErrorCliente = "El pedidio no puede ser cancelado\nYa fue procesado";
                            return View("liquidarPedidos", codigoUsuario);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorEliminar = "No se pudo cancelar el pedido\nError: " + ex.Message;
                    return View("liquidarPedidos", codigoUsuario);
                }
                ViewBag.PedidoEliminado = "Pedido fue cancelado";
                return RedirectToAction("liquidarPedidos", new { id = codigoUsuario});
            }
        }
    }
}