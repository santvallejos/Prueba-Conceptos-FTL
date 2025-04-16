using Microsoft.EntityFrameworkCore;
using Pruebas_Conceptos_MVC_FTG.Models;

namespace Pruebas_Conceptos_MVC_FTG.Data
{
    public class Pruebas_Conceptos_MVC_FTG_DbContext : DbContext
    {
        public Pruebas_Conceptos_MVC_FTG_DbContext(DbContextOptions<Pruebas_Conceptos_MVC_FTG_DbContext> options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}