using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pruebas_Conceptos_MVC_FTG.Model.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database context
builder.Services.AddDbContext<Pruebas_Conceptos_MVC_FTG_DbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

// Add Swagger to the container
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

  // Optional: Include XML comments for better documentation
  // var xmlFile = $"{System.AppDomain.CurrentDomain.FriendlyName}.xml";
  // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  // c.IncludeXmlComments(xmlPath);
});

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

if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // Enable Swagger
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión de pacientes API"); // Swagger UI endpoint
    c.RoutePrefix = string.Empty; // Make Swagger UI available at the root
  });
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Ensure database is created
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

// Example usage of the Usuario class
var usuario = new Usuario(1, "Rodrigo", "rodrigo@example.com", "secreta123");

// Verificar contraseña correcta
bool accesoPermitido = usuario.VerificarContrasena("secreta123");
Console.WriteLine($"¿Acceso permitido? {accesoPermitido}");

// Verificar contraseña incorrecta
bool accesoDenegado = usuario.VerificarContrasena("contraseñaIncorrecta");
Console.WriteLine($"¿Acceso permitido? {accesoDenegado}");

// Mostrar información del usuario
Console.WriteLine($"Nombre: {usuario.Nombre}");
Console.WriteLine($"Correo: {usuario.Correo}");
Console.WriteLine($"Creado en: {usuario.FechaCreacion}");
Console.WriteLine($"Último acceso: {usuario.FechaUltimoAcceso}");

// Actualizar último acceso y mostrarlo
usuario.ActualizarUltimoAcceso();
Console.WriteLine($"Último acceso actualizado: {usuario.FechaUltimoAcceso}");

app.Run();
