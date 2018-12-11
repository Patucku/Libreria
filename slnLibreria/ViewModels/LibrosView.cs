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
        public Sala sala { get; set; }
        public SelectList listaSalas { get; set; }
        public int selectedSala { get; set; }

        public List<Materia> materias { get; set; }
        public Materia materia { get; set; }
        public SelectList listaMaterias { get; set; }
        public int selectedMateria { get; set; }

        public List<Libro> libros { get; set; }
        public Libro libro { get; set; }
        public SelectList listaLibros { get; set; }
        public int selectedLibro { get; set; }

        public List<View_Listar_Libros_Materia_Sala_Libreria> librosMateriSalaLibreria { get; set; }
        public List<View_Listar_Libros_Materia> librosMateria { get; set; }
        public View_Listar_Libros_Materia libroMateria { get; set; }
        public List<View_Listar_Libros_Sala> librosSala { get; set; }
    }
}