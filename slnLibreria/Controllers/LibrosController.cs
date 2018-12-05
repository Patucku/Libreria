using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.ViewModels;
using slnLibreria.Models;
﻿using Microsoft.AspNet.Identity;

namespace slnLibreria.Controllers
{
    public class LibrosController : Controller
    {
        dbFeriaLibroEntities db = new dbFeriaLibroEntities();
        // GET: Libros
        public ActionResult Index()
        {
            var clienteView = Session["ClienteIngresado"] as ClientesView;
            if (clienteView == null)
                return View("Index", "Home");
            LibrosView objLibro = new LibrosView();
            var getSalas = db.Sala.OrderBy(n => n.salaNombre).ToList();
            objLibro.listaSalas = new SelectList(getSalas, "salaID", "salaNombre");

            var getMateria = db.Materia.OrderBy(n => n.materiaNombre).ToList();
            objLibro.listaMaterias = new SelectList(getMateria, "materiaID", "materiaNombre");

            return View(objLibro);
        }


        public ActionResult BuscarLibros()
        {
            var clienteView = Session["ClienteIngresado"] as ClientesView;
            if(clienteView == null)
                return View("Index", "Home");


            return View();
        }

        [HttpPost]
        public ActionResult BuscarLibros(LibrosView lv)
        {
            var clienteView = Session["ClienteIngresado"] as ClientesView;
            if (clienteView == null)
                return View("Index", "Home");

            return View();
        }
    }
}