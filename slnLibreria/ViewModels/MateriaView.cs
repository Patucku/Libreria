using slnLibreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.ViewModels
{
    public class MateriaView
    {
        public Materia materia { get; set; }

        public List<Materia> materias { get; set; }

        public List<View_Listar_Libros_Materia> librosMateria { get; set; }

        
        public SelectList materiasList { get; set; }
        public int selectedLibreria { get; set; }

    }
}