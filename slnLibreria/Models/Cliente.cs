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
    
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            this.ClienteFeriaLibro = new HashSet<ClienteFeriaLibro>();
        }
    
        public int clienteID { get; set; }
        public string clienteCI_RUC { get; set; }
        public string clienteNombre { get; set; }
        public string clienteApellido { get; set; }
        public string clienteCorreo { get; set; }
        public string clienteTelefono { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClienteFeriaLibro> ClienteFeriaLibro { get; set; }
    }
}