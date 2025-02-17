using ApiAudiencia.Models;
using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClasificacionesController : Controller
    {
        private readonly AudienciasContext _context;
        public ClasificacionesController(AudienciasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerClasificaciones()
        {
            try
            {
                var clasificaciones = await _context.Clasificaciones.ToListAsync();
                return Ok(clasificaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
