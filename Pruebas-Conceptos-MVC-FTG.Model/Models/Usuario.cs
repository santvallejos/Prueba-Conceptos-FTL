using System;

namespace Pruebas_Conceptos_MVC_FTG.Model.Models
{
  public class Usuario
  {
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string Correo { get; private set; }
    private string Contrasena { get; set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimoAcceso { get; private set; }

    public Usuario(int id, string nombre, string correo, string contrasena)
    {
      Id = id;
      Nombre = nombre;
      Correo = correo;
      Contrasena = contrasena;
      FechaCreacion = DateTime.Now;
      FechaUltimoAcceso = DateTime.Now;
    }

    public bool VerificarContrasena(string contrasena)
    {
      return Contrasena == contrasena;
    }

    public void ActualizarUltimoAcceso()
    {
      FechaUltimoAcceso = DateTime.Now;
    }
  }
}
