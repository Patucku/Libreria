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
    public class SalaController : Controller
    {
        // GET: Sala
        public ActionResult Index()
        {
            SalaView objSalaView = cargarIndex();
            return View(objSalaView);
        }

        private static SalaView cargarIndex()
        {
            SalaView objSalaView = new SalaView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSalaView.salasLibreria = db.View_Listar_Sala_Libreria.ToList();
                var getLibrerias = db.Libreria.OrderBy(n => n.libreriaNombre).ToList();
                objSalaView.librerias = new SelectList(getLibrerias, "libreriaId", "libreriaNombre");
            }

            return objSalaView;
        }

        // GET: Sala/Details/5
        public ActionResult Detalles(int ?id)
        {
            SalaView objSalaView = new SalaView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSalaView.salaLibreria = db.View_Listar_Sala_Libreria.Where(n => n.salaID == id).FirstOrDefault();
                objSalaView.sala = db.Sala.Where(n => n.salaID == id).FirstOrDefault();
                objSalaView.librosSala = db.View_Listar_Libros_Sala.Where(n => n.salaID == id).OrderBy(n=> n.libroNombre).ToList();
                if (objSalaView.salaLibreria == null)
                    ViewBag.SinLibrerias = "la sala " + objSalaView.sala.salaNombre + " no pertenece a ninguna libreria" ;
                if (objSalaView.librosSala == null)
                    ViewBag.SinLibros = "la sala " + objSalaView.sala.salaNombre + " no tienen ningún libro";
                if (objSalaView.sala == null)
                {
                    ViewBag.ErrorSala = "No se encuentra esa sala";
                    return View("Index", cargarIndex());
                }
            }
            return View(objSalaView); 

        }

        // GET: Sala/Create
        public ActionResult Crear()
        {
            SalaView objSalaView = cargarIndex();
            return View(objSalaView);
        }

        // POST: Sala/Create
        [HttpPost]
        public ActionResult Crear(SalaView objSalaView)
        {
            try
            {
                if(string.IsNullOrEmpty(objSalaView.sala.salaNombre))
                    ViewBag.ErrorCrearSala = "Ingrese un nombre";
                if(objSalaView.selectedLibreria == 0)
                    ViewBag.ErrorCrearSala = ViewBag.ErrorCrearSala + "\nSeleccione una libreria";
                if(!string.IsNullOrEmpty(ViewBag.ErrorCrearSala))
                    return View();
                else
                {
                    Sala nuevaSala = new Sala()
                    {
                        salaNombre = objSalaView.sala.salaNombre,
                        salaLibreria = objSalaView.selectedLibreria
                    };

                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        db.Sala.Add(nuevaSala);
                        db.SaveChanges();
                    }
                    ViewBag.Sala = "Se creó una nueva sala";
                    return View("Index", cargarIndex());
                }


            }
            catch(Exception ex)
            {
                ViewBag.ErrorCrearSala = "Error al ingresar la sala \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Sala/Edit/5
        public ActionResult Editar(int ?id)
        {
            SalaView objSalaView = new SalaView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSalaView.sala = db.Sala.Where(n => n.salaID == id).FirstOrDefault();
                if (objSalaView.sala == null)
                {
                    ViewBag.ErrorSala = "No se encuentra esa sala";
                    return View("Index", cargarIndex());
                }
                var getLibrerias = db.Libreria.OrderBy(n => n.libreriaNombre).ToList();
                objSalaView.librerias = new SelectList(getLibrerias, "libreriaId", "libreriaNombre", objSalaView.sala.salaLibreria);
            }
            return View(objSalaView);
        }

        // POST: Sala/Edit/5
        [HttpPost]
        public ActionResult Editar(int ?id, SalaView objSalaView)
        {
            try
            {
                Sala salaActualizar = new Sala();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    salaActualizar = db.Sala.Where(n => n.salaID == id).FirstOrDefault();
                    if (salaActualizar == null)
                    {
                        ViewBag.ErrorSala = "No se encuentra esa sala";
                        return View("Index", cargarIndex());
                    }
                    if (string.IsNullOrEmpty(objSalaView.sala.salaNombre))
                        ViewBag.ErrorCrearSala = "Ingrese un nombre";
                    if (objSalaView.selectedLibreria == 0)
                        ViewBag.ErrorCrearSala = ViewBag.ErrorCrearSala + "\nSeleccione una libreria";
                    if (!string.IsNullOrEmpty(ViewBag.ErrorCrearSala))
                        return View();
                    else
                    {
                        salaActualizar.salaNombre = objSalaView.sala.salaNombre;
                        salaActualizar.salaLibreria = objSalaView.selectedLibreria;
                        db.Entry(salaActualizar).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Sala = "Se actualizó la sala";
                        return View("Index", cargarIndex());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorActualizarSala = "Error al actualizar la sala \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Sala/Delete/5
        public ActionResult Eliminar(int ?id)
        {
            SalaView objSalaView = new SalaView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSalaView.salaLibreria = db.View_Listar_Sala_Libreria.Where(n => n.salaID == id).FirstOrDefault();
                objSalaView.librosSala = db.View_Listar_Libros_Sala.Where(n => n.salaID == id).OrderBy(n => n.libroNombre).ToList();
                if (objSalaView.salaLibreria == null)
                {
                    ViewBag.ErrorSala = "No se encuentra esa sala";
                    return View("Index", cargarIndex());
                }
                List<LibroSala> objLibrosSala = db.LibroSala.Where(n => n.salaID == objSalaView.salaLibreria.salaID).ToList();
                int librosAfectados = objLibrosSala.Count();
                if (librosAfectados != 0)
                    ViewBag.ErrorEliminarSalaRelacion = "La sala tiene: " + librosAfectados + " libros relacionados, no se puede borrar la sala";

            }
            return View(objSalaView);
        }

        // POST: Sala/Delete/5
        [HttpPost]
        public ActionResult Eliminar(int ?id, SalaView objSalaView)
        {
            try
            {
                Sala salaEliminar = new Sala();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    salaEliminar = db.Sala.Where(n => n.salaID == id).FirstOrDefault();
                    if (salaEliminar == null)
                    {
                        ViewBag.ErrorSala = "No se encuentra esa sala";
                        return View("Index", cargarIndex());
                    }
                    List<LibroSala> objLibrosSala = db.LibroSala.Where(n => n.salaID == salaEliminar.salaID).ToList();
                    int librosAfectados = objLibrosSala.Count();
                    if (librosAfectados != 0)
                    {
                        ViewBag.ErrorEliminarSalaRelacion = "La sala tiene: " + librosAfectados + " libros relacionados, no se puede borrar la sala";
                        return View(objSalaView);
                    }
                    else
                    {
                        db.Sala.Remove(salaEliminar);
                        db.SaveChanges();
                        ViewBag.Sala = "Se eliminó la sala";
                        return View("Index", cargarIndex());

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorEliminarSala = "Error al eliminar la materia \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        public ActionResult librosRelacionados(int? id)
        {
            Sala objSala = new Sala();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSala = db.Sala.Where(n => n.salaID == id).FirstOrDefault();
                if (objSala == null)
                {
                    ViewBag.ErrorSala = "No se encuentra esa sala";
                    return View("Index", cargarIndex());
                }
                SalaView objSalaView = new SalaView();
                objSalaView.librosSala = db.View_Listar_Libros_Sala.Where(n => n.salaID == objSala.salaID).OrderBy(n => n.libroNombre).ToList();
                objSalaView.sala = objSala;
                return View(objSalaView);
            }
        }
    }
}
