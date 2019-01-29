using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ClientesView clientesView = new ClientesView();
            return View(clientesView);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(ClientesView cv)
        {
            ClientesView objCliente = new ClientesView();
            if (string.IsNullOrEmpty(cv.clienteCI_RUC))
                ViewBag.ErrorCedula = "Ingrese un número de Cédula o RUC";
            else
            {
                if (cv.clienteCI_RUC.Length != 10 && cv.clienteCI_RUC.Length != 13)
                    ViewBag.ErrorCedula = "Ingrese un número de Cédula o RUC válido";
                else
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objCliente.cliente = db.Cliente.Where(n => n.clienteCI_RUC == cv.clienteCI_RUC).FirstOrDefault();
                        if (objCliente.cliente == null)
                            ViewBag.ErrorUsuarioNoCliente = "El número de Cédula o RUC: "+cv.clienteCI_RUC+" no está registrado\n";
                        else
                        {
                            ClienteLibreria objClienteLibreria = db.ClienteLibreria.Where(n => n.clienteID == objCliente.cliente.clienteID).FirstOrDefault();
                            if (objClienteLibreria == null && objCliente.cliente.clienteCodigo == null)
                                ViewBag.ErrorUsuarioNoCliente = "Usted no es Cliente de niguna libreria\n";
                            else
                            {
                                if (objCliente.cliente.clienteCodigo != null)
                                {
                                    ViewBag.ErrorCedula = "Usted ya está registrado en el sistema \n Sú codigo es: " + objCliente.cliente.clienteCodigo;
                                    ModelState.Clear();
                                    return View("Index");
                                }
                                else
                                {
                                    try
                                    {
                                        Cliente objUltimoCliente = db.Cliente.OrderByDescending(n => n.clienteFechaRegistro).FirstOrDefault();
                                        int codigoCliente = 100;
                                        if (objUltimoCliente != null)
                                            codigoCliente = objUltimoCliente.clienteCodigo.Value + 1;
                                        Cliente clienteActualizar = db.Cliente.Where(n => n.clienteCI_RUC == cv.clienteCI_RUC).FirstOrDefault();
                                        clienteActualizar.clienteFechaRegistro = DateTime.Now;
                                        clienteActualizar.clienteCodigo = codigoCliente;
                                        db.Entry(clienteActualizar).State = EntityState.Modified;
                                        db.SaveChanges();

                                        objCliente.cliente = db.Cliente.Where(n => n.clienteCodigo == codigoCliente).FirstOrDefault();
                                        ViewBag.CodigoUsuario = "Se ha generado su código. " +
                                            "\n Cliente: " + objCliente.cliente.clienteNombre + " " + objCliente.cliente.clienteApellido +
                                            "\n Su código identificador es: " + objCliente.cliente.clienteCodigo + "\nAnótelo o memorizelo, el mismo le servirá para reservar y comprar libros";

                                        ModelState.Clear();
                                        return View("Index");
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.ErrorCedula = "Ha ocurrido un error al intentar generar su código \n Error: " + ex.Message;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            objCliente = new ClientesView()
            {
                clienteCI_RUC = cv.clienteCI_RUC
            };
            Session["cedulaTemporal"] = cv.clienteCI_RUC;
            return View("Index", objCliente);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult IngresarClientes(ClientesView cv)
        {
            Cliente objClienteFeriaLibro = new Cliente();
            try
            {
                if (string.IsNullOrEmpty(cv.codigoUsuario))
                {
                    ViewBag.ErrorIngresar = "Ingrese un código";
                    return View("Index");
                }
                else
                {
                    PedidosView objPedido = new PedidosView();
                    int codigoUsuario = int.Parse(cv.codigoUsuario);
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objClienteFeriaLibro = db.Cliente.Where(n => n.clienteCodigo == codigoUsuario).FirstOrDefault();
                        if (objClienteFeriaLibro == null)
                        {
                            ViewBag.ErrorIngresar = "Debe registrase primero";
                            return View("Index");
                        }
                        else
                        {
                            Session["ClienteIngresado"] = objClienteFeriaLibro;
                            objPedido.clienteFeriaLibro = objClienteFeriaLibro;
                            return RedirectToAction("Index", "PedidosCliente", objPedido);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorIngresar = "Ha ocurrido un error al momento de ingresar \n Error: " + ex.Message;
                return View("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult GenerarClienteAnonimo()
        {
            string cv = Session["cedulaTemporal"].ToString();
            ClientesView objCliente = new ClientesView();
            if (string.IsNullOrEmpty(cv))
            {
                ViewBag.ErrorCedula = "Ingrese un número de Cédula o RUC";
                return View("Index");
            }
            else
            {
                if (cv.Length != 10 && cv.Length != 13)
                {
                    ViewBag.ErrorCedula = "Ingrese un número de Cédula o RUC válido";
                    return View("Index");
                }
                else
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objCliente.cliente = db.Cliente.Where(n => n.clienteCI_RUC == cv).FirstOrDefault();
                        if (objCliente.cliente != null)
                        {
                            if(objCliente.cliente.clienteCodigo != null)
                            {
                                ViewBag.ErrorCedula = "Usted ya está registrado en el sistema \n Sú codigo es: " + objCliente.cliente.clienteCodigo;
                                ModelState.Clear();
                                return View("Index");
                            }
                            else
                            {
                                Cliente objUltimoCliente = db.Cliente.OrderByDescending(n => n.clienteFechaRegistro).FirstOrDefault();
                                int codigoCliente = 100;
                                if (objUltimoCliente != null)
                                    codigoCliente = objUltimoCliente.clienteCodigo.Value + 1;
                                Cliente actualizarCliente = db.Cliente.Where(n => n.clienteCI_RUC == cv).FirstOrDefault();
                                actualizarCliente.clienteFechaRegistro = DateTime.Now;
                                actualizarCliente.clienteCodigo = codigoCliente;
                                db.Entry(actualizarCliente).State = EntityState.Modified;
                                db.SaveChanges();
                                ViewBag.CodigoUsuario = "Se ha generado su código. " +
                                             "\n Cliente: " + objCliente.cliente.clienteNombre + " " + objCliente.cliente.clienteApellido +
                                             "\n Su código identificador es: " + objCliente.cliente.clienteCodigo + "\nAnótelo o memorizelo, el mismo le servirá para reservar y comprar libros";

                                ModelState.Clear();
                                return View("Index");
                            }
                        }
                        else
                        {
                            try
                            {
                                Cliente objUltimoCliente = db.Cliente.OrderByDescending(n=> n.clienteFechaRegistro).FirstOrDefault();
                                int codigoCliente = 100;
                                if (objUltimoCliente != null)
                                    codigoCliente = objUltimoCliente.clienteCodigo.Value + 1;

                                Cliente nuevoCliente = new Cliente()
                                {
                                    clienteCodigo = codigoCliente,
                                    clienteCI_RUC = cv,
                                    clienteNombre = "Anónimo",
                                    clienteApellido = "N. " + codigoCliente.ToString().PadLeft(8, '0'),
                                    clienteCorreo = "anonimo@anonimo.com",
                                    clienteTelefono = "0999999999",
                                    clienteFechaRegistro = DateTime.Now
                                };
                                db.Cliente.Add(nuevoCliente);
                                db.SaveChanges();

                                ViewBag.CodigoUsuario = "Se ha generado su código. " +
                                            "\n Cliente: " + nuevoCliente.clienteNombre + " " + nuevoCliente.clienteApellido +
                                            "\n Su código identificador es: " + nuevoCliente.clienteCodigo + " Anótelo o memorizelo, el mismo le servirá para reservar y comprar libros";

                                ModelState.Clear();
                                return View("Index");
                            }
                            catch (Exception ex)
                            {
                                ViewBag.ErrorCedula = "Ha ocurrido un error al intentar generar su código como usuario anónimo \n Error: " + ex.Message;
                                return View("Index");
                            }

                        }

                    }
                }
            }

        }
    }
}
