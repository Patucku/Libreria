using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace slnLibreria.Controllers
{
    public class PedidosClienteController : Controller
    {
        // GET: PedidosCliente
        public ActionResult Index()
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction ("Index", "Home");
            }
            else
            {
                PedidosView objpedidosView = cargarIndex(usuario);
                return View(objpedidosView);
            }
        }
        
        [HttpPost]
        public ActionResult Index(PedidosView objPedido)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    var getBusqueda = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n =>
                       ((objPedido.selectedSala != 0) ? n.salaID == objPedido.selectedSala : true) &&
                       ((objPedido.selectedMateria != 0) ? n.materiaID == objPedido.selectedMateria : true) &&                       
                       ((objPedido.searchLibro != null) ? n.libroNombre.Contains(objPedido.searchLibro) : true) &&
                       n.estadoLibroSala == true &&
                       n.cantidadLibroSala > 0).
                    OrderBy(n => n.libreriaNombre);
                    objPedidoView.listaBusqueda = getBusqueda.ToList();

                    if (objPedidoView.listaBusqueda == null)
                    {
                        ViewBag.Resultados = "No existen libros en la búsqueda seleccionada";
                        return View(cargarIndex(usuario));
                    }
                    else
                    {
                        Session["ClienteIngresado"] = objPedidoView.clienteFeriaLibro;
                        Session["BusquedaLibros"] = objPedidoView;
                        RouteValueDictionary dict = new RouteValueDictionary();
                        dict.Add("sala", objPedido.selectedSala);
                        dict.Add("materia", objPedido.selectedMateria);
                        dict.Add("libro", objPedido.searchLibro);
                        return RedirectToAction("resultadoBusqueda");
                    }

                }
            }
        }

        public ActionResult resultadoBusqueda(string sala, string materia, string libro)
        {
            int selectedSala, selectedMateria;
            var usuario = Session["ClienteIngresado"] as Cliente;
            var busqueda = Session["BusquedaLibros"] as PedidosView;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario,
                listaBusqueda = busqueda.listaBusqueda
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (objPedidoView.listaBusqueda == null)
                {
                    validarParametros(sala, materia, out selectedSala, out selectedMateria);
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        var getBusqueda = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n =>
                            ((selectedSala != 0) ? n.salaID == selectedSala : true) &&
                            ((selectedMateria != 0) ? n.materiaID == selectedMateria : true) &&
                            ((libro != null) ? n.libroNombre.Contains(libro) : true) &&
                            n.estadoLibroSala == true &&
                            n.cantidadLibroSala > 0).
                        OrderBy(n => n.libreriaNombre);
                        objPedidoView.listaBusqueda = getBusqueda.ToList();
                    }
                }
                objPedidoView.clienteID = objPedidoView.clienteFeriaLibro.clienteID;
                Session["ClienteIngresado"] = objPedidoView.clienteFeriaLibro;
                Session["BusquedaLibros"] = objPedidoView;
                return View(objPedidoView);
            }
        }

        public ActionResult Detalles(int? id)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (id == null)
                return RedirectToAction("Index");
            else
            {
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objPedidoView.Libro = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n => n.librosalaID == id).FirstOrDefault();
                    if (objPedidoView.Libro == null)
                    {
                        ViewBag.ErrorCliente = "No se encuentra ese libro";
                        return View("Index", cargarIndex(usuario));
                    }
                    Variables IVA = db.Variables.Where(n => n.variableID == 1).FirstOrDefault();
                    if (objPedidoView.Libro.libroIVA.Value)
                        objPedidoView.precioTotal = objPedidoView.Libro.precioLibroSala + ((IVA.variableValorNumerico.Value / 100) * objPedidoView.Libro.precioLibroSala);
                    else
                        objPedidoView.precioTotal = objPedidoView.Libro.precioLibroSala;
                }
                return View(objPedidoView);
            }

        }

        [HttpPost]
        public ActionResult Comprar(int? id, PedidosView objPedido)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            Variables IVA = new Variables();
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objPedidoView.Libro = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n => n.librosalaID == objPedido.Libro.librosalaID).FirstOrDefault();
                        IVA = db.Variables.Where(n => n.variableID == 1).FirstOrDefault();
                    }
                    decimal subTotal = objPedido.cantidadComprada * objPedidoView.Libro.precioLibroSala;
                    decimal ivaPedido = 0;
                    if (objPedidoView.Libro.libroIVA.Value)
                        ivaPedido = (IVA.variableValorNumerico.Value / 100) * subTotal;
                    if (objPedidoView.Libro.cantidadLibroSala >= objPedido.cantidadComprada)
                    {
                        Pedido nuevoPedido = new Pedido
                        {
                            librosalaID = objPedidoView.Libro.librosalaID,
                            clienteID = objPedidoView.clienteFeriaLibro.clienteID,
                            //ESTADO PEDIDO 1 --> PEDIDO EN CURSO
                            estadopedidoID = 1,
                            cantidadPedido = objPedido.cantidadComprada,
                            subTotalPedido = subTotal,
                            ivaPedido = ivaPedido,
                            totalPedido = subTotal + ivaPedido,
                            fechaInicioPedido = DateTime.Now
                        };
                        using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                        {
                            LibroSala actualizarLibroSala = db.LibroSala.Where(n => n.librosalaID == objPedidoView.Libro.librosalaID).FirstOrDefault();
                            actualizarLibroSala.cantidadLibroSala = actualizarLibroSala.cantidadLibroSala - nuevoPedido.cantidadPedido;
                            db.Pedido.Add(nuevoPedido);
                            db.Entry(actualizarLibroSala).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        ViewBag.SuccesCliente = "Pedido Agregado";
                        return View("Index",cargarIndex(usuario));
                    }
                    else
                    {
                        ViewBag.ErrorAlComprar = "No hay suficiente inventario";
                        return View("Index", cargarIndex(usuario));

                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorAlComprar = "No se pudo realizar la compra";
                    return RedirectToAction("resultadoBusqueda");
                }
            }
        }

        //DETALLAR PEDIDOS EN CURSO
        public ActionResult detalleCompraEnCurso()
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (objPedidoView.clienteFeriaLibro.clienteID == 0)
                return RedirectToAction("Index", "Home");
            else
            {
                objPedidoView = librosEnCurso(objPedidoView);
                return View(objPedidoView);
            }

        }

        //CONFIRMAR PEDIDO POR DESPACHAR
        public ActionResult confirmarPedido(int? id)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (objPedidoView.clienteFeriaLibro.clienteID == 0)
                return RedirectToAction("Index", "Home");
            else if (id == 0)
            {
                ViewBag.ErrorFinalizar = "No existen libros en la búsqueda seleccionada";
                return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
            }
            else
            {
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        Pedido pedidoActualizar = db.Pedido.Where(n => n.pedidoID == id).FirstOrDefault();
                        if(pedidoActualizar != null && pedidoActualizar.estadopedidoID == 1)
                        {
                            pedidoActualizar.estadopedidoID = 2;
                            db.Entry(pedidoActualizar).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.SuccesFinalizar = "Pedido confirmado!";
                            return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
                        }
                        else
                        {
                            ViewBag.ErrorFinalizar = "Pedido no puede ser confirmado";
                            return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorFinalizar = "Error al ingresar: \n"+ ex.Message;
                    return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
                }
            }
        }

        //DETALLAR PEDIDIOS POR RETIRAR
        public ActionResult detalleCompraPorRetirar()
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (objPedidoView.clienteFeriaLibro.clienteID == 0)
                return RedirectToAction("Index", "Home");
            else
            {
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objPedidoView.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.estadopedidoID == 3 && n.clienteID == objPedidoView.clienteFeriaLibro.clienteID).ToList();
                }
                return View(objPedidoView);
            }
        }

        //DETALLAR PEDIDOS QUE NO ESTAN EN CURSO NI FINALZIADOS
        public ActionResult detalleLibrosCliente(PedidosView objPedido)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(librosCliente(objPedidoView));

            }

        }
        //SE BORRA DE LA BASE DE DATOS EL REGISTRO
        public ActionResult eliminarPedido(int? id)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (id == 0)
            {
                ViewBag.ErrorEliminar = "No existen libros en la búsqueda seleccionada";
                return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
            }
            else
            {
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        Pedido pedidoEliminar = db.Pedido.Where(n => n.pedidoID == id).FirstOrDefault();
                        LibroSala librosalaActualizar = db.LibroSala.Where(n => n.librosalaID == pedidoEliminar.librosalaID).FirstOrDefault();
                        if (pedidoEliminar.estadopedidoID == 1)
                        {
                            librosalaActualizar.cantidadLibroSala = librosalaActualizar.cantidadLibroSala + pedidoEliminar.cantidadPedido;
                            db.Entry(pedidoEliminar).State = EntityState.Deleted;
                            db.Entry(librosalaActualizar).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.ErrorCliente = "El pedidio no puede ser eliminado\nIntente cancelarlo";
                            return View("Index",cargarIndex(usuario));
                        }
                    }
                    ViewBag.PedidoEliminado = "Pedido fue eliminado";
                    return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorEliminar = "No se pudo eliminar el pedido\nError: " + ex.Message;
                    return View("detalleCompraEnCurso", librosEnCurso(objPedidoView));
                }

            }

        }
        //SE LE DA UN ESTADO DE PEDIDO = 5
        public ActionResult cancelarPedido(int? id)
        {
            var usuario = Session["ClienteIngresado"] as Cliente;
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            if (usuario == null)
            {
                ViewBag.ErrorIngresar = "Debe ingresar para poder realizar cualquier transacción";
                return RedirectToAction("Index", "Home");
            }
            else if (id == 0)
            {
                ViewBag.ErrorEliminar = "No existen libros en la búsqueda seleccionada";
                return View("detalleLibrosCliente", librosCliente(objPedidoView));
            }
            else
            {
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        Pedido pedidoCancelar= db.Pedido.Where(n => n.pedidoID == id).FirstOrDefault();
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
                            return View("Index",cargarIndex(usuario));
                        }
                    }
                    ViewBag.PedidoEliminado = "Pedido fue cancelado";
                    return View("detalleLibrosCliente", librosCliente(objPedidoView));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorEliminar = "No se pudo cancelar el pedido\nError: " + ex.Message;
                    return View("detalleLibrosCliente", librosCliente(objPedidoView));
                }

            }

        }

        public ActionResult salirCliente()
        {
            Session.Remove("ClienteIngresado");
            return RedirectToAction("Index", "Home");
        }

        private PedidosView cargarIndex(Cliente usuario)
        {
            PedidosView objPedidoView = new PedidosView
            {
                clienteFeriaLibro = usuario
            };
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objPedidoView.pedidosCliente = db.Pedido.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID &&
                n.estadopedidoID != 5).ToList();
                foreach (var pedido in objPedidoView.pedidosCliente)
                {
                    if (pedido.estadopedidoID == 1)
                    {
                        objPedidoView.totalEnCurso = objPedidoView.totalEnCurso + pedido.totalPedido;
                        objPedidoView.cantidadEnCurso = objPedidoView.cantidadEnCurso + pedido.cantidadPedido;
                    }
                    if (pedido.estadopedidoID == 3)
                    {
                        objPedidoView.totalPorRetirar = objPedidoView.totalPorRetirar + pedido.totalPedido;
                        objPedidoView.cantidadPorRetirar = objPedidoView.cantidadPorRetirar + pedido.cantidadPedido;
                    }
                    if (pedido.estadopedidoID >= 2 && pedido.estadopedidoID < 4)
                    {
                        objPedidoView.numeroPedidos += 1;
                        objPedidoView.cantidadComprada = objPedidoView.cantidadComprada + pedido.cantidadPedido;
                    }
                }
                var getSalas = from s in db.View_Listar_Sala_Libreria
                               select new
                               {
                                   salaId = s.salaID,
                                   nombre = s.libreriaNombre + " - " + s.salaNombre
                               };
                //db.Sala.OrderBy(n => n.salaNombre).ToList();
                /**/
                var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                objPedidoView.listSalas = new SelectList(getSalas.ToList(), "salaId", "nombre");
                objPedidoView.listMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");

            }
            Session["ClienteIngresado"] = objPedidoView.clienteFeriaLibro;
            return objPedidoView;
        }

        private PedidosView librosEnCurso(PedidosView objPedidoView)
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objPedidoView.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID && n.estadopedidoID == 1).ToList();
            }
            foreach (var pedidos in objPedidoView.DetallePedidos)
            {
                objPedidoView.totalEnCurso = objPedidoView.totalEnCurso + pedidos.totalPedido;
                objPedidoView.cantidadEnCurso = objPedidoView.cantidadEnCurso + pedidos.cantidadPedido;
            }
            return objPedidoView;
        }

        private PedidosView librosCliente(PedidosView objPedidoView)
        {
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objPedidoView.DetallePedidos = db.View_Listar_Pedidos.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID && (n.estadopedidoID ==2 || n.estadopedidoID == 3)).ToList();
                objPedidoView.DetallePedidosFinCan = db.View_Listar_Pedidos.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID && n.estadopedidoID >= 4).ToList();
            }
            return objPedidoView;
        }

        private static void validarParametros(string sala, string materia, out int selectedSala, out int selectedMateria)
        {
            if (sala == null)
                selectedSala = 0;
            else
            {
                try
                {
                    selectedSala = int.Parse(sala);
                }
                catch
                {
                    selectedSala = 0;
                }
            }
            if (materia == null)
                selectedMateria = 0;
            else
            {
                try
                {
                    selectedMateria = int.Parse(materia);
                }
                catch
                {
                    selectedMateria = 0;
                }
            }
        }

    }
}