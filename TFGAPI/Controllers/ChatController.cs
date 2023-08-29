using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFGAPI.Models;

//Sin terminar de implementar

namespace TFGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly AdopcionApiContext _context;

        public ChatController(AdopcionApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }
            return await _context.Chats.ToListAsync();
        }

        //Obtener chat por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChatByChatID(int id)
        {
            var chat = await _context.Chats.FindAsync(id);

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }

        //Obtener los chats de una publicación
        [HttpGet("pub/{pubid}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChatsByPublicacionID(int pubid)
        {
            var chat = await _context.Chats.Where(c => c.PublicacionId == pubid).ToListAsync();

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }

        //Obtener el chat de un usuario en una publicación
        [HttpGet("userchat")]
        public async Task<ActionResult<Chat>> GetChatByPublicacionID(int iniciadorid, int userid, int pubid)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.PublicacionId == pubid && c.UsuarioIniciadorId == iniciadorid && c.UsuarioPublicacionId == userid);

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }

        //Subir un nuevo mensaje
        [HttpPost]
        public async Task<ActionResult> AñadirNuevoMensaje([FromBody]Chat chat)
        {
            try
            {
                // Agregar el nuevo cat al contexto de la base de datos
                _context.Chats.Add(chat);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Chat creado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el chat: " + ex.Message);
            }
        }
    }
}
