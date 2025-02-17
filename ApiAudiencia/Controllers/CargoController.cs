using ApiAudiencia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoController : Controller
    {
        private readonly AudienciasContext _context;

        public CargoController(AudienciasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCargo()
        {
            try
            {
                var cargo = await _context.Cargos.ToListAsync();
                return Ok(cargo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
