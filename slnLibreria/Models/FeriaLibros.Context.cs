﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbFeriaLibroEntities : DbContext
    {
        public dbFeriaLibroEntities()
            : base("name=dbFeriaLibroEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClienteLibreria> ClienteLibreria { get; set; }
        public virtual DbSet<EstadoPedido> EstadoPedido { get; set; }
        public virtual DbSet<Libreria> Libreria { get; set; }
        public virtual DbSet<Libro> Libro { get; set; }
        public virtual DbSet<LibroSala> LibroSala { get; set; }
        public virtual DbSet<Materia> Materia { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Vendedor> Vendedor { get; set; }
        public virtual DbSet<View_Listar_Libros_Materia> View_Listar_Libros_Materia { get; set; }
        public virtual DbSet<View_Listar_Libros_Materia_Sala_Libreria> View_Listar_Libros_Materia_Sala_Libreria { get; set; }
        public virtual DbSet<View_Listar_Libros_Sala> View_Listar_Libros_Sala { get; set; }
        public virtual DbSet<View_Listar_Sala_Libreria> View_Listar_Sala_Libreria { get; set; }
        public virtual DbSet<Variables> Variables { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
    }
}
