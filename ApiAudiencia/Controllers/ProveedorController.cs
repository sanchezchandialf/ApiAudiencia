using Microsoft.AspNetCore.Mvc;
using ApiAudiencia.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly AudienciasContext _context;

        public ProveedorController(AudienciasContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            return await _context.Proveedores.ToListAsync();
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return proveedor;
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProveedor), new { dni = proveedor.DNI }, proveedor);
        }

// DELETE: api/proveedores/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteProveedor(int id)
{
    var proveedor = await _context.Proveedores.FindAsync(id);
    if (proveedor == null)
    {
      return NotFound();
    }
    
  try  
  { 
      _context.Remove(proveedor); 
      await _context.SaveChangesAsync(); 
  } 
  catch(DbUpdateConcurrencyException ex)  
  {   
      throw;  
  } 
    
  	return NoContent();    
}
private bool ExisteProveedorId(int dni) =>
_context.Proveedores.Any(e => e.DNI == dni);}