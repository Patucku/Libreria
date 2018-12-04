using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.Models;

namespace slnLibreria.ViewModels
{
    public class LibrosView
    {
        public List<Sala> salas { get; set; }
        public SelectList listaSalas { get; set; }
        public Sala sala { get; set; }
        public string selectedSala { get; set; }

        public List<Materia> materias { get; set; }
        public SelectList listaMaterias { get; set; }
        public Materia materia { get; set; }
        public string selectedMateria { get; set; }

        public List<Libro> libros { get; set; }
        public Libro libro { get; set; }
        public string selectedLibro { get; set; }
    }
}