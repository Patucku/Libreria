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
    
    public partial class View_Listar_Pedidos
    {
        public int pedidoID { get; set; }
        public int librosalaID { get; set; }
        public int libroID { get; set; }
        public string libroNombre { get; set; }
        public string libroAutor { get; set; }
        public string libroISBN { get; set; }
        public int libroMateria { get; set; }
        public string materiaNombre { get; set; }
        public int salaID { get; set; }
        public string salaNombre { get; set; }
        public int salaLibreria { get; set; }
        public string libreriaNombre { get; set; }
        public int clienteID { get; set; }
        public string clienteNombre { get; set; }
        public string clienteApellido { get; set; }
        public string clienteCI_RUC { get; set; }
        public string vendedorID { get; set; }
        public Nullable<bool> vendedorEstado { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int estadopedidoID { get; set; }
        public string estadopedidoNombre { get; set; }
        public int cantidadPedido { get; set; }
        public decimal subTotalPedido { get; set; }
        public decimal ivaPedido { get; set; }
        public decimal totalPedido { get; set; }
        public System.DateTime fechaInicioPedido { get; set; }
        public Nullable<System.DateTime> fechaFinPedido { get; set; }
    }
}