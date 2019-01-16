using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.ViewModels;
using slnLibreria.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace slnLibreria.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
   

        public ActionResult Reporte1()
        {
            List<Vimporte_total> objReport = cargarReporte1();
            return View(objReport);
        }


        private static List<Vimporte_total> cargarReporte1()
        {
            List<Vimporte_total> objLibreria = new List<Vimporte_total>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Vimporte_total.ToList();
            }

            return objLibreria;
        }

        public ActionResult Reporte2()
        {
            List<Vvendedores> objReport = cargarReporte2();
            return View(objReport);
        }


        private static List<Vvendedores> cargarReporte2()
        {
            List<Vvendedores> objLibreria = new List<Vvendedores>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Vvendedores.ToList();
            }

            return objLibreria;
        }
        public ActionResult Reporte3()
        {
            List<Vtitulos> objReport = cargarReporte3();
            return View(objReport);
        }


        private static List<Vtitulos> cargarReporte3()
        {
            List<Vtitulos> objLibreria = new List<Vtitulos>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Vtitulos.ToList();
            }

            return objLibreria;
        }

        public ActionResult Reporte4()
        {
            Report4 objLibro = cargarReporte4();
            return View(objLibro);
        }


        private static Report4 cargarReporte4()
        {
            Report4 objLibro = new Report4();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibro.objReport = db.VAutor.ToList();
                objLibro.objReport2 = db.VMaterias.ToList();
            }

            return objLibro;
        }

        public ActionResult Reporte5()
        {
            List<VDia> objReport = cargarReporte5();
            return View(objReport);
        }


        private static List<VDia> cargarReporte5()
        {
            List<VDia> objLibreria = new List<VDia>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.VDia.ToList();
            }

            return objLibreria;
        }
        public ActionResult Reporte6()
        {
            List<Vimporte_total> objReport = cargarReporte6();
            return View(objReport);
        }


        private static List<Vimporte_total> cargarReporte6()
        {
            List<Vimporte_total> objLibreria = new List<Vimporte_total>();
            using (dbFeriaLibroEntities db = new dbFeriaLibroEntities())
            {
                objLibreria = db.Vimporte_total.ToList();
            }

            return objLibreria;
        }

       
    }
}