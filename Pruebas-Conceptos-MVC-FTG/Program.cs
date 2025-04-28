using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Pruebas_Conceptos_MVC_FTG.Model.Models;
using Pruebas_Conceptos_MVC_FTG.Utils;

//Added to use the JWT authentication
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


// Add the following using directive at the top of the file to resolve the 'AddDbContext' method.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pruebas_Conceptos_MVC_FTG;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var key = "clave_super_secreta_para_firmar"; //CAMBIAR POR ALGO MÁS SEGURO JAJAJA solo de ensayo
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

// Add database context
builder.Services.AddDbContext<Pruebas_Conceptos_MVC_FTG_DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger to the container
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestión de Pacientes API", Version = "v1" });

  //Include XML comments for better documentation
   var xmlFile = $"{System.AppDomain.CurrentDomain.FriendlyName}.xml";
   var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

//Add authentication con JWT
builder.Services.AddAuthentication(options =>
{
    // Se indica que JWT será el esquema de autenticación por defecto
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configuración de validación del token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Se pueden validar otras cosas, como el emisor y la audiencia
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,  // Asegura que el token no haya expirado
        ValidateIssuerSigningKey = true, // Verifica la firma del token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Agregar el servicio de autenticación al contenedor de dependencias
builder.Services.AddSingleton<JwtAuthService>();

//Adding CORS para permitir solicitudes desde otros dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Permitir solicitudes desde cualquier origen, método y encabezado
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
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

app.UseCors("AllowAll");  // Habilita CORS
app.UseAuthentication();  // Activa la autenticación JWT
app.UseAuthorization(); // Activa la autorización

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión de pacientes API");
    c.RoutePrefix = string.Empty;
});

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
        db.Database.Migrate(); // Aplica las migraciones pendientes
        Console.WriteLine("✅ Conexión a la base de datos exitosa.");
  }
}
catch (Exception ex)
{
  Console.WriteLine($"❌ Error al conectar con la base de datos: {ex.Message}");
}


app.Run();
