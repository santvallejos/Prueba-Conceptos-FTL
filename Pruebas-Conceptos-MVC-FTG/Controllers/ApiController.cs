using Microsoft.AspNetCore.Mvc;
using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;

namespace Pruebas_Conceptos_MVC_FTG.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ApiController : ControllerBase
  {
    private readonly Pruebas_Conceptos_MVC_FTG_DbContext _context;

    public ApiController(Pruebas_Conceptos_MVC_FTG_DbContext context)
    {
      _context = context;
    }

    [HttpGet("pacientes")]
    public async Task<IActionResult> GetPacientes()
    {
      var pacientes = await _context.Pacientes.ToListAsync();
      return Ok(pacientes); // Devuelve el JSON directamente
    }
  }
}
