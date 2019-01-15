using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
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
                    objPedidoView.pedidosCliente = db.Pedido.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID &&
                    (n.estadopedidoID == 1 || n.estadopedidoID == 3)).ToList();
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
                    }
                    var getSalas = db.Sala.OrderBy(n => n.salaNombre).ToList();
                    var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                    objPedidoView.listSalas = new SelectList(getSalas, "salaId", "salaNombre");
                    objPedidoView.listMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");
                }
            }
            Session["ClienteIngresado"] = objPedidoView.clienteFeriaLibro;
            return View(objPedidoView);
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
                   n.estadoLibroSala == true &&
                   n.cantidadLibroSala > 0).
                    OrderBy(n => n.libreriaNombre);
                    objPedidoView.listaBusqueda = getBusqueda.ToList();
                    
                    if (objPedidoView.listaBusqueda == null)
                    {
                        var getSalas = db.Sala.OrderBy(n => n.salaNombre).ToList();
                        var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                        objPedidoView.listSalas = new SelectList(getSalas, "salaId", "salaNombre");
                        objPedidoView.listMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");
                        ViewBag.Resultados = "No existen libros en la búsqueda seleccionada";
                        return View(objPedidoView);
                    }
                    else
                    {
                        Session["ClienteIngresado"] = objPedidoView.clienteFeriaLibro;
                        Session["BusquedaLibros"] = objPedidoView;
                        RouteValueDictionary dict = new RouteValueDictionary();
                        dict.Add("sala", objPedido.selectedSala);
                        dict.Add("materia", objPedido.selectedMateria);
                        return RedirectToAction("resultadoBusqueda");
                    }

                }
            }
        }
        public ActionResult resultadoBusqueda(string sala, string materia)
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
                       n.estadoLibroSala == true &&
                       n.cantidadLibroSala > 0).
                        OrderBy(n => n.libreriaNombre);
                        objPedidoView.listaBusqueda = getBusqueda.ToList();

                        objPedidoView.pedidosCliente = db.Pedido.Where(n => n.clienteID == objPedidoView.clienteFeriaLibro.clienteID &&
                    (n.estadopedidoID == 1 || n.estadopedidoID == 3)).ToList();
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
                        }
                    }
                }

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
                        ViewBag.ErrorLibro = "No se encuentra ese libro";
                        return RedirectToAction("Index");
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
                    if (objPedidoView.Libro.cantidadLibroSala > objPedido.cantidadComprada)
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
                            actualizarLibroSala.cantidadLibroSala = actualizarLibroSala.cantidadLibroSala - objPedidoView.cantidadComprada;
                            db.Pedido.Add(nuevoPedido);
                            db.Entry(actualizarLibroSala).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        ViewBag.Sala = "Pedido Agregado";
                        return RedirectToAction("Index");
                    }else
                    {
                        ViewBag.ErrorAlComprar = "No hay suficiente inventario";
                        return View();

                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorAlComprar = "No se pudo realizar la compra";
                    return RedirectToAction("resultadoBusqueda");
                }
            }


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