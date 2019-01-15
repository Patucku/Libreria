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
                if (cv.clienteCI_RUC.Length == 9 || cv.clienteCI_RUC.Length == 12)
                    ViewBag.ErrorCedula = "Ingrese un número de Cédula o RUC válido";
                else
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objCliente.cliente = db.Cliente.Where(n => n.clienteCI_RUC == cv.clienteCI_RUC).FirstOrDefault();
                        if (objCliente.cliente == null)
                            ViewBag.ErrorCedula = "Usted no es Cliente";
                        else
                        {
                            ClienteLibreria objClienteLibreria = db.ClienteLibreria.Where(n => n.clienteID == objCliente.cliente.clienteID).FirstOrDefault();
                            if(objClienteLibreria == null)
                                ViewBag.ErrorCedula = "Usted no es Cliente de niguna libreria";
                            else
                            {
                                if (objCliente.cliente.clienteCodigo != null)
                                {
                                    ViewBag.ErrorCedula = "Usted ya está registrado en el sistema \n Sú codigo es: " + objCliente.cliente.clienteCodigo;
                                    ModelState.Clear();
                                }
                                else
                                {
                                    try
                                    {
                                        Cliente objUltimoCliente = db.Cliente.Where(n => n.clienteCodigo != null).Last();
                                        int codigoCliente = 100;
                                        if (objUltimoCliente != null)
                                            codigoCliente = objUltimoCliente.clienteCodigo.Value + 1;
                                        Cliente objClienteFeriaLibro = new Cliente();
                                        objClienteFeriaLibro.clienteFechaRegistro = DateTime.Now;
                                        objClienteFeriaLibro.clienteCodigo = codigoCliente;
                                        db.Entry(objClienteFeriaLibro).State = EntityState.Modified;
                                        db.SaveChanges();

                                        objCliente.cliente = db.Cliente.Where(n => n.clienteCodigo == codigoCliente).FirstOrDefault();
                                        ViewBag.CodigoUsuario = "Se ha generado su código. " +
                                            "\n Cliente: " + objCliente.cliente.clienteNombre + " " + objCliente.cliente.clienteApellido +
                                            "\n Su código identificador es: " + objCliente.cliente.clienteCodigo + " Anótelo o memorizelo, el mismo le servirá para reservar y comprar libros";

                                        PedidosView objPedido = new PedidosView();
                                        Session["ClienteIngresado"] = objCliente.cliente;
                                        objPedido.clienteFeriaLibro = objCliente.cliente;
                                        ModelState.Clear();
                                        return View("Index", "Pedidos", objPedido);
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
            objCliente = new ClientesView();
            return View("Index");
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
                    return View();
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
                        }
                    }
                    return RedirectToAction("Index", "PedidosCliente", objPedido);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorIngresar = "Ha ocurrido un error al momento de ingresar \n Error: " + ex.Message;
                return View("Index");
            }
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
