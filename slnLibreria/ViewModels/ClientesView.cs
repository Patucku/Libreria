using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using slnLibreria.Models;

namespace slnLibreria.ViewModels
{
    public class ClientesView
    {
        public string clienteCI_RUC { get; set; }

        public Cliente cliente {get; set;}

        public string codigoUsuario { get; set; }
       
    }
}