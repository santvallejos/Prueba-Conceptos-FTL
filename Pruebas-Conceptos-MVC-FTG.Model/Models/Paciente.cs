using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas_Conceptos_MVC_FTG.Models
{
  public class Paciente
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public int Identification { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;


    public Paciente(int id, string name, string surname, DateTime birthdate, int identification, string diagnosis, string institution)
    {
      Id = id;
      Name = name;
      Surname = surname;
      Birthdate = birthdate;
      Identification = identification;
      Diagnosis = diagnosis;
      Institution = institution;
    }



  }
}
