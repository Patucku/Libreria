using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.Models;

namespace slnLibreria.ViewModels
{
    public class SalaView
    {
        public Sala sala { get; set; }

        public List<Sala> salas { get; set; }


        public List<View_Listar_Libros_Sala> librosSala { get; set; }

        public View_Listar_Sala_Libreria salaLibreria { get; set; }
        public List<View_Listar_Sala_Libreria> salasLibreria { get; set; }
        public SelectList librerias { get; set; }
        public int selectedLibreria { get; set; }

        public Libreria libreria { get; set; }

    }
}