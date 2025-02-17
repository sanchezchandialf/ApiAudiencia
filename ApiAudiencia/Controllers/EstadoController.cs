using ApiAudiencia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : Controller
    {
        private readonly AudienciasContext _context;

        public EstadoController(AudienciasContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerEstado()
        {
            try
            {
                var estado = await _context.Estados.ToListAsync();
                return Ok(estado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
