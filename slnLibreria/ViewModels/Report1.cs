using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnLibreria.ViewModels;
using slnLibreria.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace slnLibreria.ViewModels
{
    public class Report4
    {
        public List<VAutor> objReport { get; set; }
        public List<VMaterias> objReport2 { get; set; }

        
    }
}