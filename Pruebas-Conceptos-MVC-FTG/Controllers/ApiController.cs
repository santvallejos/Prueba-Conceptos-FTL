using Microsoft.AspNetCore.Mvc;
using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.EntityFrameworkCore;
using Pruebas_Conceptos_MVC_FTG.Models;
using System.Text.Json;
using Pruebas_Conceptos_MVC_FTG.Utils;

namespace Pruebas_Conceptos_MVC_FTG.Controllers
{
    /// <summary>
    /// Controlador API para manejar operaciones sobre pacientes.
    /// </summary>
    [Route("api/[controller]")]
  [ApiController]
  public class ApiController : ControllerBase
  {
    private readonly Pruebas_Conceptos_MVC_FTG_DbContext _context;

    public ApiController(Pruebas_Conceptos_MVC_FTG_DbContext context)
    {
      _context = context;
    }
     
    // Endpoint para obtener la lista de pacientes
    [HttpGet("pacientes")]
    public async Task<IActionResult> GetPacientes()
    {
            try
            {
                // Obtiene la lista de pacientes de la base de datos
                var pacientes = await _context.Pacientes.ToListAsync();

                // Devuelve los pacientes en formato JSON
                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve un BadRequest
                return BadRequest($"Error al obtener los pacientes: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un nuevo paciente en la base de datos.
        /// </summary>
        /// <param name="nuevoPaciente">Datos del paciente a crear.</param>
        /// <returns>El paciente creado.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePaciente([FromBody] Paciente nuevoPaciente)
        {
            if (nuevoPaciente == null)
            {
                return BadRequest("El paciente no puede ser nulo.");
            }

            try
            {
                _context.Pacientes.Add(nuevoPaciente);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPacienteById), new { id = nuevoPaciente.Id }, nuevoPaciente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el paciente: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un paciente específico por su ID.
        /// </summary>
        /// <param name="id">ID del paciente a buscar.</param>
        /// <returns>Paciente encontrado o un mensaje de error.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPacienteById(int id)
        {
            try
            {
                // Buscar paciente en la base de datos por ID
                var paciente = await _context.Pacientes.FindAsync(id);

                // Si no se encuentra el paciente
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Si se encuentra el paciente, se retorna con el estado 200 OK
                return Ok(paciente);
            }
            catch (Exception ex)
            {
                // En caso de error, se maneja la excepción
                return BadRequest($"Error al obtener el paciente: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza la información de un paciente existente.
        /// </summary>
        /// <param name="id">ID del paciente a actualizar.</param>
        /// <param name="pacienteActualizado">Nuevo estado del paciente.</param>
        /// <returns>Respuesta HTTP indicando el éxito o error de la actualización.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, [FromBody] Paciente pacienteActualizado)
        {
            if (id != pacienteActualizado.Id)
            {
                return BadRequest("El ID del paciente en la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            var pacienteExistente = await _context.Pacientes.FindAsync(id);
            if (pacienteExistente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            // Aquí se podría realizar la lógica para actualizar los campos específicos
            pacienteExistente.Name = pacienteActualizado.Name;
            pacienteExistente.Surname = pacienteActualizado.Surname;
            pacienteExistente.Birthdate = pacienteActualizado.Birthdate;
            pacienteExistente.Identification = pacienteActualizado.Identification;
            pacienteExistente.Diagnosis = pacienteActualizado.Diagnosis;
            pacienteExistente.Institution = pacienteActualizado.Institution;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve un código 204 si la actualización es exitosa
            }

            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el paciente: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza parcialmente la información de un paciente existente.
        /// </summary>
        /// <param name="id">ID del paciente a actualizar.</param>
        /// <param name="camposActualizados">Campos a modificar.</param>
        /// <returns>Paciente actualizado o mensaje de error.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPaciente(int id, [FromBody] JsonElement camposActualizados)
        {
            var pacienteExistente = await _context.Pacientes.FindAsync(id);
            if (pacienteExistente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            try
            {
                // Deserializar parcialmente los datos al objeto existente
                var pacienteJson = JsonSerializer.Serialize(pacienteExistente);
                var docOriginal = JsonDocument.Parse(pacienteJson);
                var objMerged = JsonUtils.MergeJson(docOriginal.RootElement, camposActualizados);

                var pacienteModificado = JsonSerializer.Deserialize<Paciente>(objMerged.ToString());

                // Actualizar los campos en el paciente existente
                _context.Entry(pacienteExistente).CurrentValues.SetValues(pacienteModificado);
                await _context.SaveChangesAsync();

                return Ok(pacienteExistente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar parcialmente el paciente: {ex.Message}");
            }
        }


        /// <summary>
        /// Elimina un paciente de la base de datos.
        /// </summary>
        /// <param name="id">ID del paciente a eliminar.</param>
        /// <returns>Respuesta HTTP indicando el éxito o error de la eliminación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            try
            {
                var paciente = await _context.Pacientes.FindAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el paciente: {ex.Message}");
            }
        }
    }
}
