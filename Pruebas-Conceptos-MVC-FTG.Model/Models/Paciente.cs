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
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public int Identification { get; set; }
        public string Diagnosis { get; set; }
        public string Institution { get; set; }
    }
}