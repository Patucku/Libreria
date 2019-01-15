using slnLibreria.Models;
using slnLibreria.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class LibrosSalasController : Controller
    {
        // GET: LibroSala/Details/5
        public ActionResult Detalles(int? id)
        {
            if (id == 0 || id == null)
            {
                ViewBag.ErrorRelacionarLibroSala = "Seleccione una relación";
                return View("Relacionar", cargarDatosRelacion());
            }
            else
            {
                LibrosSalasView objLibroSala = new LibrosSalasView();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objLibroSala.libroComplete = db.View_Listar_Libros_Materia_Sala_Libreria.Where(n => n.librosalaID == id).FirstOrDefault();
                }
                if (objLibroSala == null)
                {
                    ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                    return View("Relacionar", cargarDatosRelacion());
                }
                else
                {
                    objLibroSala.estadoLibroSala = objLibroSala.libroComplete.estadoLibroSala;
                    return View(objLibroSala);
                }
            }
        }

        // GET: LibroSala/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == 0 || id == null)
            {
                ViewBag.ErrorRelacionarLibroSala = "Seleccione una relación";
                return View("Relacionar", cargarDatosRelacion());
            }
            else
            {
                LibrosSalasView objLibroSala = new LibrosSalasView();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objLibroSala.libroSala = db.View_Listar_Libros_Sala.Where(n => n.librosalaID == id).FirstOrDefault();
                }
                if (objLibroSala == null)
                {
                    ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                    return View("Relacionar", cargarDatosRelacion());
                }
                else
                {
                    objLibroSala.estadoLibroSala = objLibroSala.libroSala.estadoLibroSala;
                    return View(objLibroSala);
                }
            }
        }

        // POST: LibroSala/Edit/5
        [HttpPost]
        public ActionResult Editar(int? id, LibrosSalasView objActualizar)
        {
            try
            {
                if (id != 0 || id != null)
                {
                    LibroSala objactualizarRelacion = new LibroSala();
                    if (objActualizar.libroSala.cantidadLibroSala < 0)
                        ViewBag.ErrorAcRelacionarLibroSala = ViewBag.ErrorAcRelacionarLibroSala + "\nIngrese una cantidad positiva";
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objactualizarRelacion = db.LibroSala.Where(n =>
                        n.librosalaID == objActualizar.libroSala.librosalaID).FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(ViewBag.ErrorRelacionarLibroSala))
                        return View(cargarDatosRelacion());
                    else
                    {
                        objactualizarRelacion.precioLibroSala = objActualizar.libroSala.precioLibroSala;
                        objactualizarRelacion.cantidadLibroSala = objActualizar.libroSala.cantidadLibroSala;
                        objactualizarRelacion.estadoLibroSala = objActualizar.estadoLibroSala;
                        using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                        {
                            db.Entry(objactualizarRelacion).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.RelacionarLibroSala = "Se actualizó la relacion";
                            ModelState.Clear();
                            return RedirectToAction("Relacionar", ViewBag);
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorAcRelacionarLibroSala = "Seleccione una relación";
                    return View(objActualizar);

                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorAcRelacionarLibroSala = "Error al editar la relación,\n" + ex.Message;
                return View(objActualizar);
            }
        }

        // GET: LibroSala/Delete/5
        public ActionResult Eliminar(int? id)
        {
            if (id == 0 || id == null)
            {
                ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                return View("Relacionar", cargarDatosRelacion());
            }
            else
            {
                View_Listar_Libros_Sala objLibroSalaEliminar = new View_Listar_Libros_Sala();
                Pedido objPedido = new Pedido();
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    objLibroSalaEliminar = db.View_Listar_Libros_Sala.Where(n => n.librosalaID == id).FirstOrDefault();
                    objPedido = db.Pedido.Where(n => n.librosalaID == id).FirstOrDefault();
                }
                if (objLibroSalaEliminar == null)
                {
                    ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                    return View("Relacionar", cargarDatosRelacion());
                }
                else if (objPedido != null)
                {
                    ViewBag.PedidoRelacionados = "No se puede eliminar la relación mantiene alguna dependencia\nSe recomienda desactivar la relación";
                }
                return View(objLibroSalaEliminar);
            }
        }

        // POST: LibroSala/Delete/5
        [HttpPost]
        public ActionResult Eliminar(int? id, View_Listar_Libros_Materia objAEliminar)
        {
            try
            {
                LibroSala objLibroSalaEliminar = new LibroSala();
                Pedido objPedido = new Pedido();
                if (id != 0 || id != null)
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objLibroSalaEliminar = db.LibroSala.Where(n => n.librosalaID == id).FirstOrDefault();
                        objPedido = db.Pedido.Where(n => n.librosalaID == id).FirstOrDefault();
                        if (objLibroSalaEliminar == null)
                            ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                        else if (objPedido == null)
                        {
                            db.LibroSala.Remove(objLibroSalaEliminar);
                            db.SaveChanges();
                            ViewBag.RelacionarLibroSala = "Se eliminó la relación";
                        }
                    }
                }
                else
                    ViewBag.ErrorRelacionarLibroSala = "Seleccione una relación";
                ModelState.Clear();
                return RedirectToAction("Relacionar", ViewBag);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorElRelacionarLibroSala = "Sucedió un error al eliminar la relación\n" +ex.Message;
                return View(objAEliminar);
            }
        }

        public ActionResult EliminarRepetido(int? id, LibrosSalasView objAEliminar)
        {
            try
            {
                LibroSala objLibroSalaEliminar = new LibroSala();
                Pedido objPedido = new Pedido();
                if (id != 0 | id != null)
                {
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        objLibroSalaEliminar = db.LibroSala.Where(n => n.librosalaID == id).FirstOrDefault();
                        objPedido = db.Pedido.Where(n => n.librosalaID == id).FirstOrDefault();
                        if (objLibroSalaEliminar == null)
                        {
                            ViewBag.ErrorRelacionarLibroSala = "No existe la relación";
                        }
                        else if (objPedido == null)
                        {
                            db.LibroSala.Remove(objLibroSalaEliminar);
                            db.SaveChanges();
                            ViewBag.RelacionarLibroSala = "Se eliminó la relación";
                        }
                        else
                            ViewBag.ErrorRelacionarLibroSala = "No se puede eliminar la relación mantiene alguna dependencia\nSe recomienda desactivar la relación";
                    }
                }
                return RedirectToAction("Relacionar", ViewBag);
            }
            catch
            {
                ViewBag.ErrorRelacionarLibroSala = "Sucedió un error al eliminar la relación";
                return RedirectToAction("Relacionar", ViewBag);
            }
        }

        public ActionResult Relacionar()
        {
            LibrosSalasView objLibroSala = cargarDatosRelacion();
            return View(objLibroSala);
        }

        [HttpPost]
        public ActionResult Relacionar(LibrosSalasView objLibroSala)
        {
            try

            {
                LibrosSalasView nuevaRelacion = new LibrosSalasView();
                if (objLibroSala.selectedLibro == 0)
                    ViewBag.ErrorRelacionarLibroSala = "Seleccione un Libro";
                if (objLibroSala.selectedSala == 0)
                    ViewBag.ErrorRelacionarLibroSala = ViewBag.ErrorRelacionarLibroSala + "\nSeleccione una Sala";
                if (objLibroSala.libroSala.cantidadLibroSala < 0)
                    ViewBag.ErrorRelacionarLibroSala = ViewBag.ErrorRelacionarLibroSala + "\nIngrese una cantidad positiva";
                using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                {
                    nuevaRelacion.LibrosSalas = db.View_Listar_Libros_Sala.Where(n =>
                    n.salaID == objLibroSala.selectedSala &&
                    n.libroID == objLibroSala.selectedLibro).ToList();
                }
                if (!string.IsNullOrEmpty(ViewBag.ErrorRelacionarLibroSala))
                    return View(cargarDatosRelacion());
                else
                {
                    LibroSala nuevoLibroSala = new LibroSala()
                    {
                        libroID = objLibroSala.selectedLibro,
                        salaID = objLibroSala.selectedSala,
                        cantidadLibroSala = objLibroSala.libroSala.cantidadLibroSala,
                        precioLibroSala = objLibroSala.libroSala.precioLibroSala,
                        estadoLibroSala = true
                    };
                    using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
                    {
                        db.LibroSala.Add(nuevoLibroSala);
                        db.SaveChanges();
                    }
                    if (nuevaRelacion.LibrosSalas.Count() != 0)
                    {
                        ViewBag.RelacionarLibroSalaExsinte = "Se agregó la nueva relación\nPero ya existe una con el mismo libro y la misma sala\nDesea eliminarla?";
                        ModelState.Clear();
                        return View(cargarDatosRelacion(nuevoLibroSala.librosalaID));
                    }
                    else
                        ViewBag.RelacionarLibroSala = "Se agregó la nueva relación";
                }
                ModelState.Clear();
                return View(cargarDatosRelacion());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorRelacionarLibroSala = "Error al ingresar relación,\n" + ex.Message;
                return View(cargarDatosRelacion());
            }
        }


        private static LibrosSalasView cargarDatosRelacion()
        {
            LibrosSalasView objLibroSala = new LibrosSalasView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibroSala.LibrosSalas = db.View_Listar_Libros_Sala.OrderBy(n => n.salaNombre).ToList();
                var getLibros = db.Libro.OrderBy(n => n.libroNombre).ToList();
                var getSalas = db.Sala.OrderBy(n => n.salaNombre).ToList();
                objLibroSala.listLibros = new SelectList(getLibros, "libroId", "libroNombre");
                objLibroSala.listSalas = new SelectList(getSalas, "salaId", "salaNombre");
            }
            return objLibroSala;
        }
        private static LibrosSalasView cargarDatosRelacion(int librosalaID)
        {
            LibrosSalasView objLibroSala = new LibrosSalasView();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibroSala.LibrosSalas = db.View_Listar_Libros_Sala.OrderBy(n => n.salaNombre).ToList();
                var getLibros = db.Libro.OrderBy(n => n.libroNombre).ToList();
                var getSalas = db.Sala.OrderBy(n => n.salaNombre).ToList();
                objLibroSala.listLibros = new SelectList(getLibros, "libroId", "libroNombre");
                objLibroSala.listSalas = new SelectList(getSalas, "salaId", "salaNombre");
                objLibroSala.libroSala = db.View_Listar_Libros_Sala.Where(n => n.librosalaID == librosalaID).FirstOrDefault();
            }
            return objLibroSala;
        }
    }
}
