using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pruebas_Conceptos_MVC_FTG.Models;
using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;

namespace Pruebas_Conceptos_MVC_FTG.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Pruebas_Conceptos_MVC_FTG_DbContext _context;

    public HomeController(ILogger<HomeController> logger, Pruebas_Conceptos_MVC_FTG_DbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> GetPacientes()
    {
        try
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return Json(pacientes); // Devuelve como JSON
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener los pacientes: {ex.Message}");
        }
    }
}
