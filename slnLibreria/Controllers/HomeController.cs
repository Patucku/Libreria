using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using slnLibreria.ViewModels;
using slnLibreria.Models;

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
                            ViewBag.ErrorCedula = "Usted no es Cliente de ninguna libreria";
                        else
                        {
                            objCliente.clienteFeriaLibro = db.ClienteFeriaLibro.Where(n => n.clienteferialibro_clienteID == objCliente.cliente.clienteID).FirstOrDefault();
                            if (objCliente.clienteFeriaLibro != null)
                                ViewBag.ErrorCedula = "Usted ya está registrado en el sistema \n Sú codigo es: " + objCliente.clienteFeriaLibro.clienteferialibroID;
                            else
                            {
                                try
                                {
                                    objCliente.clienteFeriaLibro.clienteferialibroFechaCreacion = DateTime.Now;
                                    objCliente.clienteFeriaLibro.clienteferialibro_clienteID = objCliente.cliente.clienteID;
                                    db.ClienteFeriaLibro.Add(objCliente.clienteFeriaLibro);
                                    db.SaveChanges();

                                    objCliente.clienteFeriaLibro = db.ClienteFeriaLibro.Where(n => n.clienteferialibro_clienteID == objCliente.cliente.clienteID).FirstOrDefault();
                                    ViewBag.CodigoUsuario = "Se ha generado su código. " +
                                        "\n Cliente: " + objCliente.cliente.clienteNombre + " " + objCliente.cliente.clienteApellido +
                                        "\n Su código identificador es: " + objCliente.clienteFeriaLibro.clienteferialibroID + " Anótelo o memorizelo, el mismo le servirá para reservar y comprar libros";

                                    Session["ClienteIngresado"] = objCliente;
                                    return RedirectToAction("BuscarLibro", "Libros", objCliente);
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
            return View(objCliente);
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
