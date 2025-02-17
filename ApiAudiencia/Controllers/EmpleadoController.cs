using ApiAudiencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : Controller
    {

        private readonly AudienciasContext _context;
        public EmpleadoController(AudienciasContext context)
        {
            _context = context;
        }
        [HttpGet]
        
        public async Task<IActionResult> ObtenerEmpleados()
        {
          
            try
            {
                var empleados = await _context.Empleados.ToListAsync();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        
    }
}
