using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TFGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeController : ControllerBase
    {
        private readonly AdopcionApiContext _context;

        public MensajeController(AdopcionApiContext context)
        {
            _context = context;
        }

        [HttpGet("{chatid}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensajesByChatID(int chatid)
        {
            var mensaje = await _context.Mensajes.Where(c => c.ChatId == chatid).ToListAsync();

            if (mensaje == null)
            {
                return NotFound();
            }

            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> AñadirMensjae([FromBody] Mensaje msg)
        {
            try
            {
                // Agregar el nuevo mensaje al contexto de la base de datos
                _context.Mensajes.Add(msg);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Mensaje creado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el mensaje: " + ex.Message);
            }
        }
    }
}
