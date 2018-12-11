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
    public class MateriaController : Controller
    {   // GET: Materia
        public ActionResult Index()
        {
            List<Materia> objMaterias = cargarIndex();
            return View(objMaterias);
        }

        private static List<Materia> cargarIndex()
        {
            List<Materia> objMaterias = new List<Materia>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objMaterias = db.Materia.ToList();
            }

            return objMaterias;
        }

        // GET: Materia/Details/5
        public ActionResult Detalles(int ?id)
        {
            Materia objMateria = new Materia();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objMateria = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                if (objMateria == null)
                {
                    ViewBag.ErrorMateria = "No se encuentra esa materia";
                    return View("Index", cargarIndex());
                }
            }
            return View(objMateria);
        }

        // GET: Materia/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Materia/Create
        [HttpPost]
        public ActionResult Crear(Materia objMateria)
        {
            try
            {
                if (string.IsNullOrEmpty(objMateria.materiaNombre))
                {
                    ViewBag.ErrorCrearMateria = "Ingrese un nombre";
                    return View();
                }
                else
                {
                    Materia nuevaMateria = new Materia()
                    {
                        materiaNombre = objMateria.materiaNombre
                    };
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        db.Materia.Add(objMateria);
                        db.SaveChanges();
                    }
                    ViewBag.Materia = "Se creó una nueva materia";
                    return View("Index", cargarIndex());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCrearMateria = "Error al ingresar la materia \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Materia/Edit/5
        public ActionResult Editar(int ?id)
        {
            Materia objMateria = new Materia();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objMateria = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                if (objMateria == null)
                {
                    ViewBag.ErrorMateria = "No se encuentra esa materia";
                    return View("Index", cargarIndex());
                }
            }
            return View(objMateria);

        }

        // POST: Materia/Edit/5
        [HttpPost]
        public ActionResult Editar(int ?id, Materia objMateria)
        {
            try
            {
                Materia materiaActualizar = new Materia();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    materiaActualizar = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                    if (materiaActualizar == null)
                    {
                        ViewBag.ErrorMateria = "No se encuentra esa materia";
                        return View("Index", cargarIndex());
                    }
                    if (string.IsNullOrEmpty(objMateria.materiaNombre))
                    {
                        ViewBag.ErrorActualizarMateria = "Ingrese un nombre";
                        return View();
                    }
                    else
                    {
                        materiaActualizar.materiaNombre = objMateria.materiaNombre;
                        db.Entry(materiaActualizar).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Materia = "Se actualizó la materia";
                        return View("Index", cargarIndex());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorActualizarMateria = "Error al actualizar la materia \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        // GET: Materia/Delete/5
        public ActionResult Eliminar(int ?id)
        {
            Materia objMateria = new Materia();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objMateria = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                if (objMateria == null)
                {
                    ViewBag.ErrorMateria = "No se encuentra esa materia";
                    return View("Index", cargarIndex());
                }
                List<Libro> objLibros = db.Libro.Where(n => n.libroMateria == objMateria.materiaID).ToList();
                int librosAfectados = objLibros.Count();
                if (librosAfectados != 0)
                    ViewBag.ErrorEliminarMateriaRelacion = "La materia tiene: " + librosAfectados + " libros relacionados, no se puede borrar la materia";
            }
            return View(objMateria);
        }

        // POST: Materia/Delete/5
        [HttpPost]
        public ActionResult Eliminar(int ?id, Materia objMateria)
        {
            try
            {
                Materia materiaEliminar = new Materia();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    materiaEliminar = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                    if (materiaEliminar == null)
                    {
                        ViewBag.ErrorMateria = "No se encuentra esa materia";
                        return View("Index", cargarIndex());
                    }
                    List<Libro> objLibros = db.Libro.Where(n => n.libroMateria == materiaEliminar.materiaID).ToList();
                    int librosAfectados = objLibros.Count();
                    if (librosAfectados != 0)
                    {
                        ViewBag.ErrorEliminarMateriaRelacion = "La materia tiene: " + librosAfectados + " libros relacionados, no se puede borrar la materia";
                        return View(objMateria);
                    }
                    else
                    {
                        db.Materia.Remove(materiaEliminar);
                        db.SaveChanges();
                        ViewBag.Materia = "Se eliminó la materia";
                        return View("Index", cargarIndex());

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorEliminarMateria = "Error al eliminar la materia \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        public ActionResult librosRelacionados(int ?id)
        {
            Materia objMateria = new Materia();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objMateria = db.Materia.Where(n => n.materiaID == id).FirstOrDefault();
                if (objMateria == null)
                {
                    ViewBag.ErrorLibreria = "No se encuentra esa materia";
                    return View("Index", cargarIndex());
                }
                MateriaView objMateriaView = new MateriaView();
                objMateriaView.librosMateria = db.View_Listar_Libros_Materia.Where(n => n.materiaID == objMateria.materiaID).OrderBy(n => n.libroNombre).ToList();
                objMateriaView.materia = objMateria;
                return View(objMateriaView);
            }
        }
    }
}