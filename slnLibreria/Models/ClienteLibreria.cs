//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace slnLibreria.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClienteLibreria
    {
        public int clientelibreria_clienteID { get; set; }
        public int clientelibreria_libreriaID { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Libreria Libreria { get; set; }
    }
}
