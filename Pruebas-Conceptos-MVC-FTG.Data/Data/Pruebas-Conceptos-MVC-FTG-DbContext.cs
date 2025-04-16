using Microsoft.EntityFrameworkCore;
using Pruebas_Conceptos_MVC_FTG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas_Conceptos_MVC_FTG.Data
{
    public class Pruebas_Conceptos_MVC_FTG_DbContext : DbContext
    {
        public Pruebas_Conceptos_MVC_FTG_DbContext(DbContextOptions<Pruebas_Conceptos_MVC_FTG_DbContext> options) : base(options)
        {
        }

        //Entidades
        public DbSet<Paciente> Pacientes { get; set; }

        //Modelo a crear
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //Configuracion de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}