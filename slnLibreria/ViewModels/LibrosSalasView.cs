using slnLibreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.ViewModels
{
    public class LibrosSalasView
    {
        public List<Sala> salas { get; set; }
        public Sala sala { get; set; }
        public SelectList listSalas { get; set; }
        public int selectedSala { get; set; }


        public List<Libro> libros { get; set; }
        public Libro libro { get; set; }
        public SelectList listLibros { get; set; }
        public int selectedLibro { get; set; }

        public List<View_Listar_Libros_Materia_Sala_Libreria> LibrosComplete { get; set; }
        public View_Listar_Libros_Materia_Sala_Libreria  libroComplete { get; set; }
        public List<View_Listar_Libros_Sala> LibrosSalas { get; set; }
        public View_Listar_Libros_Sala libroSala { get; set; }

        public bool estadoLibroSala { get; set; }
    }
}