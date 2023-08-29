using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFGAPI.Models;

namespace TFGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {

        private readonly AdopcionApiContext _context;

        public LogInController(AdopcionApiContext context)
        {
            _context = context;
        }

        //Funcion que gestiona el inicio de sesión
        [HttpPost]
        public async Task<ActionResult> IniciarSesion([FromBody]LogInParameters credentials)
        {
            try
            {
                // Validar los parámetros de inicio de sesión
                if (string.IsNullOrEmpty(credentials.EmailUsuario) || string.IsNullOrEmpty(credentials.Password))
                {
                    return BadRequest("Debe proporcionar un correo electrónico y una contraseña válidos.");
                }

                // Buscar al usuario en la base de datos
                var usuario =  _context.Usuarios.FirstOrDefault(u => u.Email == credentials.EmailUsuario);

                // Verificar si el usuario existe y si la contraseña es correcta
                if (usuario == null || usuario.Password != credentials.Password)
                {
                    return Unauthorized("Credenciales de inicio de sesión inválidas.");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al iniciar sesión: " + ex.Message);
            }
        }
    }    
}
