using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pruebas_Conceptos_MVC_FTG.Data
{
    public class Pruebas_Conceptos_MVC_FTG_DbContextFactory : IDesignTimeDbContextFactory<Pruebas_Conceptos_MVC_FTG_DbContext>
    {
        public Pruebas_Conceptos_MVC_FTG_DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Pruebas_Conceptos_MVC_FTG_DbContext>();
            
            var connectionString = "Server=shortline.proxy.rlwy.net;Port=12779;Database=railway;User ID=root;Password=ptmFRprrgGmVZVUzYMxjlwpaQvbPkPjQ;";
            
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new Pruebas_Conceptos_MVC_FTG_DbContext(optionsBuilder.Options);
        }
    }
}
