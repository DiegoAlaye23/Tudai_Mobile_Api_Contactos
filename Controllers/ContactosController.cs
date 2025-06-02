using ContApi.Models;
using GestionContactosApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionContactosApi.Controllers
{
    [ApiController]
    [Route("api/contactos")]
    public class ContactosController : ControllerBase
    {
        private readonly DbA358b2Pam3Context _context;

        public ContactosController(DbA358b2Pam3Context context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            return await _context.Contacto.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Contacto>> GetContacto(int id)
        {
            var contacto = await _context.Contacto.FindAsync(id);
            if (contacto == null)
                return NotFound();

            return contacto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Contacto>> PostContacto(Contacto contacto)
        {


            _context.Contacto.Add(contacto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContacto), new { id = contacto.ContactoId }, contacto);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutContacto(int id, Contacto contacto)
        {
            if (id != contacto.ContactoId)
                return BadRequest();

            _context.Entry(contacto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contacto.Any(e => e.ContactoId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContacto(int id)
        {
            var contacto = await _context.Contacto.FindAsync(id);
            if (contacto == null)
                return NotFound();

            _context.Contacto.Remove(contacto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
