using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFGAPI.Models;

namespace TFGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //Controlador que trata las pubicaciones
    public class PublicacionController : ControllerBase
    {
        private readonly AdopcionApiContext _context;

        public PublicacionController(AdopcionApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicaciones()
        {
            if (_context.Publicaciones == null)
            {
                return NotFound();
            }
            return await _context.Publicaciones.ToListAsync();
        }

        //Obtener publicaciones de usuarios distintos a userid
        [HttpGet("otros/{userid}")]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicacionesOtros(int userid)
        {
            if (_context.Publicaciones == null)
            {
                return NotFound();
            }            
            return  Ok(_context.Publicaciones.Where(p => p.UsuarioId != userid).ToList());
        }

        //Obtener publicacion or id de publicacion
        [HttpGet("{id}")]
        public async Task<ActionResult<Publicacion>> GetPublicacionByID(int id)
        {
            var publi = await _context.Publicaciones.FindAsync(id);

            if (publi == null)
            {
                return NotFound();
            }

            return publi;
        }

        //Obtener todas las publicaciones del usuario
        [HttpGet("user/{userid}")]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicacionesByUsuario(int userid)
        {      
            try
            {
                var publi = await _context.Publicaciones.Where(p => p.UsuarioId == userid).ToListAsync();

                if (publi == null)
                {
                    return NotFound();
                }

                return publi;
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las publicaciones: " + ex.Message);
            }
        }

        //Obtener las publicaciones favoritas del usuario
        [HttpGet("fav/{userid}")]
        public async Task<ActionResult<IEnumerable<Publicacion>>> GetPublicacionesFavoritasByUsuario(int userid)
        {
            try
            {
                // Obtener todas las IDs de las publicaciones favoritas del usuario
                var idsPublicacionesFavoritas = await _context.PublicacionesFavoritas
                    .Where(pf => pf.UsuarioId == userid)
                    .Select(pf => pf.PublicacionId)
                    .ToListAsync();

                // Obtener las publicaciones completas basadas en las IDs obtenidas
                var publicacionesFavoritas = await _context.Publicaciones
                    .Where(p => idsPublicacionesFavoritas.Contains(p.PublicacionId))
                    .ToListAsync();

                return Ok(publicacionesFavoritas);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las publicaciones favoritas: " + ex.Message);
            }
        }
        //Comprobar si una publicación está en favoritos del usuario
        [HttpGet("fav/{userid}/{publid}")]
        public async Task<ActionResult<bool>> CombrobarFavorito(int userid, int publid)
        {
            try
            {
                bool res = false;
                if (_context.PublicacionesFavoritas.Count() == 0)
                {
                    return Ok(res);
                }

                res = await _context.PublicacionesFavoritas.AnyAsync(p => p.UsuarioId == userid && p.PublicacionId == publid);               

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener la publicacion favorita: " + ex.Message);
            }
        }

        //Añade una publicación a favoritos
        [HttpPost("fav/{userid}/{publid}")]
        public async Task<ActionResult> AñadirNuevaPublicacionFavorita(int userid, int publid)
        {
            try
            {
                bool publi = false;
                if (_context.PublicacionesFavoritas.Count() > 0)
                    publi = _context.PublicacionesFavoritas.Any(p => p.UsuarioId == userid && p.PublicacionId == publid);

                if (publi)
                {
                    return BadRequest("Ya está en favoritos");
                }

                PublicacionesFavoritas pfav = new PublicacionesFavoritas
                {
                    UsuarioId = userid,
                    PublicacionId = publid
                };
                // Agregar la nueva publicacion al contexto de la base de datos
                _context.PublicacionesFavoritas.Add(pfav);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Se ha guardado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al vinculadar la publicacion: " + ex.Message);
            }
        }
        //Borra una publicacion de favoritos
        [HttpDelete("fav/{userid}/{publid}")]
        public async Task<ActionResult> DeletePublicacionFavoritos(int userid, int publid)
        {
            try
            {
                if (_context.PublicacionesFavoritas.Count() == 0)
                {
                    return NotFound("No hay elementos en la lista");
                }

                var publi = await _context.PublicacionesFavoritas.SingleAsync(p => p.UsuarioId == userid && p.PublicacionId == publid);

                if (publi == null)
                {
                    return NotFound("No hay encontrado en la lista");
                }
                // Agregar la nueva publicacion al contexto de la base de datos
                _context.PublicacionesFavoritas.Remove(publi);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Publicacion desvinculada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al desvincular la publicacion: " + ex.Message);
            }
        }

        //Añade una nueva publicacion
        [HttpPost]
        public async Task<ActionResult> AñadirNuevaPublicacion([FromBody] Publicacion publi)
        {
            try
            {
                DateTime dt = DateTime.Now;
                publi.FechaPublicacion = dt;
                publi.NumInteresados = 0;
                // Agregar la nueva publicacion al contexto de la base de datos
                _context.Publicaciones.Add(publi);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(_context.Publicaciones.FirstOrDefault(p => p.UsuarioId == publi.UsuarioId && p.FechaPublicacion == dt));
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar la publicacion: " + ex.Message);
            }
        }

        //Modificar publicacion existente
        [HttpPut]
        public async Task<IActionResult> CambiarPublicacion([FromBody] Publicacion publi)
        {
            try
            {
                // Buscar la publicacion en la base de datos
                var publicacionExistente = _context.Publicaciones.FirstOrDefault(u => u.PublicacionId == publi.PublicacionId);

                // Verificar si la publicacion existe
                if (publicacionExistente == null)
                {
                    return NotFound("No se encontró la publicacion especificada");
                }

                // Actualizar las propiedades de la publicacion existente con los valores de la publicacion modificado
                publicacionExistente.TamanyoAnimal = publi.TamanyoAnimal;
                publicacionExistente.SexoAnimal = publi.SexoAnimal;
                publicacionExistente.NumInteresados = publi.NumInteresados;
                publicacionExistente.Ciudad = publi.Ciudad;
                publicacionExistente.EdadAnimal = publi.EdadAnimal;
                publicacionExistente.FotoAnimal = publi.FotoAnimal;
                publicacionExistente.EspecieAnimal = publi.EspecieAnimal;
                publicacionExistente.Descripcion = publi.Descripcion;
                publicacionExistente.NombreAnimal = publi.NombreAnimal;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(_context.Publicaciones.FirstOrDefault(p => p.PublicacionId == publi.PublicacionId));
            }
            catch (Exception ex)
            {
                return BadRequest("Error al modificar el publicacion: " + ex.Message);
            } 
        }

        //Función para borrar una publicación por id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublicacion(int id)
        {
            try
            {
                var publi = _context.Publicaciones.FirstOrDefault(p => p.PublicacionId == id);

                if (publi == null)
                {
                    return NotFound();
                }
                // Agregar la nueva publicacion al contexto de la base de datos
                _context.Publicaciones.Remove(publi);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                var publi2 = _context.Publicaciones.FirstOrDefault(p => p.PublicacionId == id);

                bool borrado = false;

                if(publi2 == null)
                {
                    borrado = true;
                }

                return Ok(borrado);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al borrar la publicacion: " + ex.Message);
            }
        }
    }
}
