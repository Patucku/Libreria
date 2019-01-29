using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class LibrosController : Controller
    {
        // GET: Libros
        public ActionResult Index()
        {
            LibrosView objLibro = cargarIndex();
            return View(objLibro);
        }

        private static LibrosView cargarIndex()
        {
            LibrosView objLibro = new LibrosView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibro.libros = db.Libro.OrderBy(n => n.libroNombre).ToList();
                var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                objLibro.listaMaterias = new SelectList(getMaterias, "libreriaId", "libreriaNombre");
            }

            return objLibro;
        }

        public ActionResult Detalles(int? id)
        {
            LibrosView objLibroView = new LibrosView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibroView.libro = db.Libro.Where(m => m.libroID == id).FirstOrDefault();
                objLibroView.imagenesLibros = db.Imagen.Where(n => n.imagenLibro == id).ToList();
                objLibroView.libroMateria = db.View_Listar_Libros_Materia.Where(n => n.libroID == id).FirstOrDefault();
                objLibroView.librosMateriSalaLibreria = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n => n.libroID == id).ToList();
                if (objLibroView.librosMateriSalaLibreria == null)
                    ViewBag.SinSalas = "El libro " + objLibroView.libroMateria.libroNombre + " no se encuentra en ninguna sala";
                if (objLibroView.libroMateria == null)
                {
                    ViewBag.ErrorLibro = "No se encuentra ese libro";
                    return View("Index", cargarIndex());
                }
                if (objLibroView.libro.libroIVA != null)
                    objLibroView.aplicaIVA = objLibroView.libro.libroIVA.Value;
                else
                    objLibroView.aplicaIVA = false;
            }
            return View(objLibroView);
        }

        public ActionResult Crear()
        {
            LibrosView objLibro = new LibrosView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                objLibro.listaMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");
            }
            return View(objLibro);
        }

        [HttpPost]
        public ActionResult Crear(LibrosView objLibroView, HttpPostedFileBase[] file)
        {
            if (string.IsNullOrEmpty(objLibroView.libro.libroNombre))
                ViewBag.ErrorCrearLibro = "Ingrese un nombre";
            if (string.IsNullOrEmpty(objLibroView.libro.libroAutor))
                ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese un autor";
            if (string.IsNullOrEmpty(objLibroView.libro.libroISBN))
                ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese un código ISBN";
            if (string.IsNullOrEmpty(objLibroView.libro.libroSinopsis))
                ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese una sinopsis";
            if (objLibroView.selectedMateria == 0)
                ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nSeleccione una materia";
            if (file[0] == null && file[1] == null && file[2] == null)
                ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nCargue por lo menos una imagen";
            if (!string.IsNullOrEmpty(ViewBag.ErrorCrearLibro))
            {
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                    objLibroView.listaMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");
                    return View(objLibroView);
                }
            }
            else
            {
                Libro ultimoLibro = new Libro();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    ultimoLibro = db.Libro.OrderByDescending(n => n.libroID).FirstOrDefault();
                }
                Libro nuevoLibro = new Libro()
                {
                    libroNombre = objLibroView.libro.libroNombre,
                    libroAutor = objLibroView.libro.libroAutor,
                    libroISBN = objLibroView.libro.libroISBN,
                    libroIVA = objLibroView.aplicaIVA,
                    libroSinopsis = objLibroView.libro.libroSinopsis,
                    libroMateria = objLibroView.selectedMateria
                };
                int libroID = ultimoLibro.libroID + 1;
                List<Imagen> imagenesLibro = subirImagen(file, libroID);
                try
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        
                        db.Libro.Add(nuevoLibro);
                        db.SaveChanges();
                        foreach (Imagen nuevaImagen in imagenesLibro)
                        {
                            if (nuevaImagen != null)
                                db.Imagen.Add(nuevaImagen);
                        }
                        db.SaveChanges();
                    }
                    ViewBag.Libro = "Se creó un nuevo libro";
                    return View("Index", cargarIndex());
                }
                catch (Exception ex)
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        ViewBag.ErrorCrearLibro = "Error al ingresar libro\n" + ex.Message;
                        var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                        objLibroView.listaMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre");
                        return View(objLibroView);
                    }
                }
            }

        }

        public ActionResult Editar(int? id)
        {
            LibrosView objLibroView = new LibrosView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibroView.libro = db.Libro.Where(n => n.libroID == id).FirstOrDefault();
                if (objLibroView.libro == null)
                {
                    ViewBag.ErrorLibro = "No se encuentra ese libro";
                    return View("Index", cargarIndex());
                }
                if (objLibroView.libro.libroIVA != null)
                    objLibroView.aplicaIVA = objLibroView.libro.libroIVA.Value;
                else
                    objLibroView.aplicaIVA = false;
                var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                objLibroView.selectedMateria = objLibroView.libro.libroMateria;
                objLibroView.listaMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre", objLibroView.libro.libroMateria);
            }
            return View(objLibroView);

        }

        [HttpPost]
        public ActionResult Editar(int? id, LibrosView objLibroView)
        {
            try
            {
                Libro libroActualizar = new Libro();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    libroActualizar = db.Libro.Where(n => n.libroID == id).FirstOrDefault();
                    if (libroActualizar == null)
                    {
                        ViewBag.ErrorLibro = "No se encuentra ese libro";
                        return View("Index", cargarIndex());
                    }
                    if (string.IsNullOrEmpty(objLibroView.libro.libroNombre))
                        ViewBag.ErrorCrearLibro = "Ingrese un nombre";
                    if (string.IsNullOrEmpty(objLibroView.libro.libroAutor))
                        ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese un autor";
                    if (string.IsNullOrEmpty(objLibroView.libro.libroISBN))
                        ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese un código ISBN";
                    if (string.IsNullOrEmpty(objLibroView.libro.libroSinopsis))
                        ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nIngrese una sinopsis";
                    if (objLibroView.selectedMateria == 0)
                        ViewBag.ErrorCrearLibro = ViewBag.ErrorCrearLibro + "\nSeleccione una materia";
                    if (!string.IsNullOrEmpty(ViewBag.ErrorCrearLibro))
                    {
                        var getMaterias = db.Materia.OrderBy(n => n.materiaNombre).ToList();
                        objLibroView.listaMaterias = new SelectList(getMaterias, "materiaId", "materiaNombre", objLibroView.libro.libroMateria);
                        return View(objLibroView);
                    }
                    else
                    {

                        libroActualizar.libroNombre = objLibroView.libro.libroNombre;
                        libroActualizar.libroAutor = objLibroView.libro.libroAutor;
                        libroActualizar.libroISBN = objLibroView.libro.libroISBN;
                        libroActualizar.libroIVA = objLibroView.aplicaIVA;
                        libroActualizar.libroSinopsis = objLibroView.libro.libroSinopsis;
                        libroActualizar.libroMateria = objLibroView.selectedMateria;
                        db.Entry(libroActualizar).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Sala = "Se actualizó la el libro";
                        return View("Index", cargarIndex());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorActualizarSala = "Error al actualizar el libro \n " +
                    "Error: " + ex.Message;
                return View();
            }
        }

        public ActionResult Eliminar(int? id)
        {
            LibrosView objLibroView = new LibrosView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibroView.libro = db.Libro.Where(n => n.libroID == id).FirstOrDefault();
                objLibroView.libroMateria = db.View_Listar_Libros_Materia.Where(n => n.libroID == id).FirstOrDefault();
                objLibroView.librosSala = db.View_Listar_Libros_Sala.Where(n => n.libroID == id).ToList();
                if (objLibroView.librosSala == null)
                    ViewBag.SinSalas = "El libro " + objLibroView.libroMateria.libroNombre + " no se encuentra en ningun libro";
                if (objLibroView.libroMateria == null)
                {
                    ViewBag.ErrorSala = "No se encuentra ese libro";
                    return View("Index", cargarIndex());
                }
                List<LibroSala> objSalasLibro = db.LibroSala.Where(n => n.libroID == objLibroView.libroMateria.libroID).ToList();
                int salasAfectadas = objSalasLibro.Count();
                if (salasAfectadas != 0)
                    ViewBag.ErrorEliminarLibroRelacion = "El libro tiene: " + salasAfectadas + " salas relacionadas, no se puede borrar el libro";

            }
            return View(objLibroView);

        }

        [HttpPost]
        public ActionResult Eliminar(int? id, LibrosView objLibroView)
        {
            try
            {
                Libro libroEliminar = new Libro();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    libroEliminar = db.Libro.Where(n => n.libroID == id).FirstOrDefault();
                    if (libroEliminar == null)
                    {
                        ViewBag.ErrorLibro = "No se encuentra ese libro";
                        return View("Index", cargarIndex());
                    }
                    List<LibroSala> objSalasLibro = db.LibroSala.Where(n => n.libroID == libroEliminar.libroID).ToList();
                    int librosAfectados = objSalasLibro.Count();
                    if (librosAfectados != 0)
                    {
                        ViewBag.ErrorEliminarLibroRelacion = "La sala tiene: " + librosAfectados + " libros relacionados, no se puede borrar la sala";
                        return View(objLibroView);
                    }
                    else
                    {
                        db.Libro.Remove(libroEliminar);
                        db.SaveChanges();
                        ViewBag.Sala = "Se eliminó el libro";
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

        public ActionResult salasRelacionadas(int? id)
        {
            LibrosSalasView objSalaLibro = new LibrosSalasView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objSalaLibro.libro = db.Libro.Where(n => n.libroID == id).FirstOrDefault();
                if (objSalaLibro.libro == null)
                {
                    ViewBag.ErrorLibreria = "No se encuentra esa libreria";
                    return View("Index", cargarIndex());
                }
                objSalaLibro.LibrosComplete = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n => n.libroID == objSalaLibro.libro.libroID).OrderBy(n => n.salaNombre).ToList();
                return View(objSalaLibro);
            }
        }

        private List<Imagen> subirImagen(HttpPostedFileBase[] imagenes, int id)
        {
            try
            {
                List<Imagen> imagenesAGuardar = new List<Imagen>();
                foreach (HttpPostedFileBase file in imagenes)
                {

                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {

                            var fileName = Path.GetFileName(file.FileName);
                            var extension = Path.GetExtension(fileName);

                            //VALIDACIONES
                            if (extension.ToUpper() != ".JPG" && extension.ToUpper() != ".JPEG" && extension.ToUpper() != ".PNG")
                            {
                                ViewBag.ErrorCierre = "\nError al Cargar Informe. Solo puede subir achivos JPG, JPEG o PNG.";
                                return null;
                            }

                            //string serverpath = Server.MapPath("~/App_Data/Informes");
                            string referencia = "LIBR" + id.ToString("D6");
                            string serverfile = Path.Combine(Server.MapPath("~/App_Data/images"), referencia);

                            int cont = 1;
                            string texto = "";

                            while (System.IO.File.Exists(serverfile + texto + extension))
                            {
                                texto = "_" + cont.ToString();
                                cont++;
                            }

                            serverfile += texto + extension;

                            file.SaveAs(serverfile);

                            Imagen objImagen = new Imagen()
                            {
                                imagenLibro = id,
                                imagenArchivo = referencia + texto + extension
                            };
                            imagenesAGuardar.Add(objImagen);
                        }
                        else
                        {
                            ViewBag.ErrorCierre = ViewBag.ErrorCierre + "\nDebe seleccionar un Archivo Válido.";
                            return null;
                        }
                    }
                }
                return imagenesAGuardar;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCierre = ViewBag.ErrorCierre + "\nError al algún archivo\n " + ex.Message;
                return null;
            }

        }

    }
}