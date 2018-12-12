using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.Controllers
{
    public class LibroSalaController : Controller
    {
        // GET: LibroSala
        public ActionResult Index()
        {
            return View();
        }

        // GET: LibroSala/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibroSala/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroSala/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LibroSala/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibroSala/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LibroSala/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibroSala/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
