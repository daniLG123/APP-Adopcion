using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TFGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AdopcionApiContext _context;

        public UsuarioController(AdopcionApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            return await _context.Usuarios.ToListAsync();
        }

        //Obtener usuario por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByID(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //Obtener los usuarios que son protectoras
        [HttpGet("protectoras/{id}")]
        public async Task<ActionResult<List<Usuario>>> GetProtectoras(int id)
        {
            var user = await _context.Usuarios.Where(u => u.EsProtectora == "S" && u.UsuarioId != id).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //Función para registrar usuario
        [HttpPost]
        public async Task<ActionResult> RegistrarUsuario([FromBody]Usuario user)
        {
            try
            {
                // Buscar al usuario en la base de datos
                var us = _context.Usuarios.FirstOrDefault(u => u.Email == user.Email);

                if(us == null)
                {
                    // Agregar el nuevo usuario al contexto de la base de datos
                    _context.Usuarios.Add(user);

                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync(); 

                    return Ok(_context.Usuarios.FirstOrDefault(u => u.Email == user.Email));
                }
                else
                {
                    return BadRequest("Email ya registrado");
                }

                
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el usuario: " + ex.Message);
            }
        }

        //Función para modificar usuario
        [HttpPut]
        public async Task<IActionResult> ModificarPerfilUsuario([FromBody] Usuario user)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == user.UsuarioId);

                // Verificar si el usuario existe
                if (usuarioExistente == null)
                {
                    return NotFound("No se encontró el perfil especificado");
                }

                // Actualizar las propiedades del usuario existente con los valores del usuario modificado
                usuarioExistente.Username = user.Username;
                usuarioExistente.Email = user.Email;
                usuarioExistente.Password = user.Password;
                usuarioExistente.Telefono = user.Telefono;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Perfil modificado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al modificar el perfil: " + ex.Message);
            }
        }

    }
}
