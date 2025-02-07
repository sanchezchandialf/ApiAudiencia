using Microsoft.AspNetCore.Mvc;
using ApiAudiencia.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly AudienciasContext _context;

        public EmpleadoController(AudienciasContext context)
        {
            _context = context;
        }

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            return await _context.Empleados.ToListAsync();
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }

        // POST: api/Empleados
        [HttpPost]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.DNI }, empleado);
        }
        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmoji(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
            return NotFound();
            }
            
        try  
        { 
            _context.Remove(empleado); 
            await _context.SaveChangesAsync(); 
        } 
        catch(DbUpdateConcurrencyException ex)  
        {   
            throw;  
        } 
            
            return NoContent();    
        }
private bool EmpleadoExists(int id) =>
_context.Empleados.Any(e => e.DNI == id);}