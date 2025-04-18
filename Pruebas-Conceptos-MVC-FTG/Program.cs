using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database context
builder.Services.AddDbContext<Pruebas_Conceptos_MVC_FTG_DbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

try
{
  using (var scope = app.Services.CreateScope())
  {
    var db = scope.ServiceProvider.GetRequiredService<Pruebas_Conceptos_MVC_FTG_DbContext>();
    db.Database.EnsureCreated(); // O db.Database.CanConnect()
    Console.WriteLine("✅ Conexión a la base de datos exitosa.");
  }
}
catch (Exception ex)
{
  Console.WriteLine($"❌ Error al conectar con la base de datos: {ex.Message}");
}

app.Run();
