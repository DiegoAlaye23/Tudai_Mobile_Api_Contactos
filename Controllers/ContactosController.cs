using System.Text;
using GestionContactosApi.Models;
using GestionContactosApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace GestionContactosApi.Controllers
{
    [ApiController]
    [Route("api/contacto")]
    public class ContactosController : ControllerBase
    {
        private readonly ContactoService _servicio;

        public ContactosController(ContactoService servicio)
        {
            _servicio = servicio;
        }

        // GET: /api/contactos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacto>> ObtenerPorId(int id)
        {
            var contacto = await _servicio.ObtenerPorId(id);
            if (contacto == null)
                return NotFound();

            return Ok(contacto);
        }

        // POST: /api/contactos/add
        [HttpPost("add")]
        public async Task<ActionResult<Contacto>> Agregar(Contacto nuevo)
        {
            var exito = await _servicio.Agregar(nuevo);
            if (!exito)
                return StatusCode(500, "No se pudo agregar el contacto.");

            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
        }

        // PUT: /api/contactos/edit/{id}
        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Editar(int id, Contacto actualizado)
        {
            var contactoExistente = await _servicio.ObtenerPorId(id);
            if (contactoExistente == null)
                return NotFound();

            // En Supabase, no se puede hacer update desde GET
            actualizado.Id = id;
            var exito = await _servicio.Editar(actualizado);
            if (!exito)
                return StatusCode(500, "No se pudo actualizar el contacto.");

            return NoContent();

        }

        // Opcional: GET todos
        [HttpGet]
        public async Task<ActionResult<List<Contacto>>> ObtenerTodos()
        {
            var lista = await _servicio.ObtenerTodos();
            return Ok(lista);
        }

        // DELETE: /api/contactos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var exito = await _servicio.Eliminar(id);
            if (!exito)
                return NotFound();

            return NoContent();
        }
    }



}
