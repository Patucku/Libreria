using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.Models;
using slnLibreria.ViewModels;

namespace slnLibreria.Controllers
{
    public class LibreriaController : Controller
    {
        // GET: Libreria
        public ActionResult Index()
        {
            List<Libreria> objLibreria = cargarIndex();
            return View(objLibreria);
        }

        private static List<Libreria> cargarIndex()
        {
            List<Libreria> objLibreria = new List<Libreria>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Libreria.ToList();
            }

            return objLibreria;
        }

        // GET: Libreria/Details/5
        public ActionResult Detalles(int ?id)
        {
            Libreria objLibreria = new Libreria();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                if (objLibreria == null)
                {
                    ViewBag.ErrorLibreria = "No se encuentra esa libreria";
                    return View("Index",cargarIndex());
                }
            }
            return View(objLibreria);
        }

        // GET: Libreria/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Libreria/Create
        [HttpPost]
        public ActionResult Crear(Libreria objLibreria)
        {
            try
            {
                if (string.IsNullOrEmpty(objLibreria.libreriaNombre))
                    ViewBag.ErrorCrearlibreria = "Ingrese un nombre";
                if (string.IsNullOrEmpty(objLibreria.libreriaRUC))
                    ViewBag.ErrorCrearlibreria = ViewBag.ErrorCrearlibreria + "\nIngrese un número de cédula o ruc";
                else if (objLibreria.libreriaRUC.Length != 10 && objLibreria.libreriaRUC.Length != 13)
                    ViewBag.ErrorCrearlibreria = ViewBag.ErrorCrearlibreria + "\nIngrese una cédula o ruc válida";
                if (string.IsNullOrEmpty(objLibreria.libreriaTelefono))
                    ViewBag.ErrorCrearlibreria = ViewBag.ErrorCrearlibreria + "\nIngrese un  teléfono";
                else if (objLibreria.libreriaTelefono.Length != 8)
                    ViewBag.ErrorCrearlibreria = ViewBag.ErrorCrearlibreria + "\nIngrese un  teléfono valido por ejmplo: 23331470";
                if (string.IsNullOrEmpty(objLibreria.libreriaDireccion))
                    ViewBag.ErrorCrearlibreria = ViewBag.ErrorCrearlibreria + "\nIngrese una dirección";
                if (!string.IsNullOrEmpty(ViewBag.ErrorCrearlibreria))
                    return View();
                else
                {
                    Libreria nuevaLibreria = new Libreria()
                    {
                        libreriaNombre = objLibreria.libreriaNombre,
                        libreriaRUC = objLibreria.libreriaRUC,
                        libreriaTelefono = objLibreria.libreriaTelefono,
                        libreriaDireccion = objLibreria.libreriaDireccion
                    };
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        db.Libreria.Add(objLibreria);
                        db.SaveChanges();
                    }
                    ViewBag.libreria = "Se creó una nueva libreria";
                    return View("Index", cargarIndex());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCrearlibreria = "Error al ingresar la libreria \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Libreria/Edit/5
        public ActionResult Editar(int ?id)
        {
            Libreria objLibreria = new Libreria();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                if (objLibreria == null)
                {
                    ViewBag.Errorlibreria = "No se encuentra esa libreria";
                    return View("Index", cargarIndex());
                }
            }
            return View(objLibreria);
        }

        // POST: Libreria/Edit/5
        [HttpPost]
        public ActionResult Editar(int ?id, Libreria objLibreria)
        {
            try
            {
                Libreria libreriaActualizar = new Libreria();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    libreriaActualizar = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                    if (libreriaActualizar == null)
                    {
                        ViewBag.Errorlibreria = "No se encuentra esa libreria";
                        return View("Index", cargarIndex());
                    }
                    if (string.IsNullOrEmpty(objLibreria.libreriaNombre))
                        ViewBag.ErrorActualizarLibreria = "Ingrese un nombre";
                    if (string.IsNullOrEmpty(objLibreria.libreriaRUC))
                        ViewBag.ErrorActualizarLibreria = ViewBag.ErrorActualizarLibreria + "\nIngrese un número de cédula o ruc";
                    else if (objLibreria.libreriaRUC.Length != 10 && objLibreria.libreriaRUC.Length != 13)
                        ViewBag.ErrorActualizarLibreria = ViewBag.ErrorActualizarLibreria + "\nIngrese una cédula o ruc válida";
                    if (string.IsNullOrEmpty(objLibreria.libreriaTelefono))
                        ViewBag.ErrorActualizarLibreria = ViewBag.ErrorActualizarLibreria + "\nIngrese un  teléfono";
                    else if (objLibreria.libreriaTelefono.Length != 8)
                        ViewBag.ErrorActualizarLibreria = ViewBag.ErrorActualizarLibreria + "\nIngrese un  teléfono valido";
                    if (string.IsNullOrEmpty(objLibreria.libreriaDireccion))
                        ViewBag.ErrorActualizarLibreria = ViewBag.ErrorActualizarLibreria + "\nIngrese una dirección";
                    if (!string.IsNullOrEmpty(ViewBag.ErrorActualizarLibreria))
                        return View();
                    else
                    {
                        libreriaActualizar.libreriaNombre = objLibreria.libreriaNombre;
                        libreriaActualizar.libreriaRUC = objLibreria.libreriaRUC;
                        libreriaActualizar.libreriaTelefono = objLibreria.libreriaTelefono;
                        libreriaActualizar.libreriaDireccion = objLibreria.libreriaDireccion;
                        db.Entry(libreriaActualizar).State = EntityState.Modified;
                        db.SaveChanges();

                        ViewBag.libreria = "Se actualizó la libreria";                        
                        return View("Index", cargarIndex());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorActualizarLibreria = "Error al actualizar la libreria \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Libreria/Delete/5
        public ActionResult Eliminar(int ?id)
        {
            Libreria objLibreria = new Libreria();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                if (objLibreria == null)
                {
                    ViewBag.ErrorLibreria = "No se encuentra esa libreria";
                    return View("Index", cargarIndex());
                }
                List<Sala> objSalas = db.Sala.Where(n => n.salaLibreria == objLibreria.libreriaID).ToList();
                int salasAfectadas = objSalas.Count();
                if (salasAfectadas != 0)
                    ViewBag.ErrorEliminarLibreriaRelacion = "La libreria tiene: " + salasAfectadas + " salas relacionadas, no se puede borrar la libreria";
            }
            return View(objLibreria);
        }

        // POST: Libreria/Delete/5
        [HttpPost]
        public ActionResult Eliminar(int ?id, Libreria objLibreria)
        {
            try
            {
                Libreria libroEliminar = new Libreria();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    libroEliminar = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                    if (libroEliminar == null)
                    {
                        ViewBag.Errorlibreria = "No se encuentra esa libreria";
                        return View("Index", cargarIndex());
                    }
                    List<Sala> objSalas = db.Sala.Where(n => n.salaLibreria == libroEliminar.libreriaID).ToList();
                    int salasAfectadas = objSalas.Count();
                    if (salasAfectadas != 0)
                    {
                        ViewBag.ErrorEliminarLibreriaRelacion = "La libreria tiene: " + salasAfectadas + " salas relacionadas, no se puede borrar la libreia";
                        return View(objLibreria);
                    }
                    else
                    {
                        db.Libreria.Remove(libroEliminar);
                        db.SaveChanges();
                        ViewBag.Libreria = "Se eliminó la liberia";
                        return View("Index", cargarIndex());

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorEliminarLibreria = "Error al eliminar la libreria \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        public ActionResult salasRelacionadas(int? id)
        {
            Libreria objLibreria = new Libreria();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Libreria.Where(n => n.libreriaID == id).FirstOrDefault();
                if (objLibreria == null)
                {
                    ViewBag.ErrorLibreria = "No se encuentra esa libreria";
                    return View("Index", cargarIndex());
                }
                SalaView objSalaView = new SalaView();
                objSalaView.salasLibreria = db.View_Listar_Sala_Libreria.Where(n => n.libreriaID == objLibreria.libreriaID).OrderBy(n => n.salaNombre).ToList();
                objSalaView.libreria = objLibreria;
                return View(objSalaView);
            }
        }
    }
}
