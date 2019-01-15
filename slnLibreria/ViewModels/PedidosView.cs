﻿using slnLibreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnLibreria.ViewModels
{
    public class PedidosView
    {
        public Cliente clienteFeriaLibro { get; set; }

        public SelectList listSalas { get; set; }
        public int selectedSala { get; set; }

        public SelectList listMaterias { get; set; }
        public int selectedMateria { get; set; }

        public View_Listar_Libros_Materia_Sala_Libreria Libro { get; set; }

        public List<View_Listar_Libros_Materia_Sala_Libreria> listaBusqueda { get; set; }

        public decimal precioTotal { get; set; }
        public int cantidadComprada { get; set; }

        public List<Pedido> pedidosCliente { get; set; }
        public decimal totalEnCurso {get; set;}
        public int cantidadEnCurso { get; set; }

        public decimal totalPorRetirar { get; set; }
        public int cantidadPorRetirar { get; set; }

    }
}