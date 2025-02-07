using Microsoft.AspNetCore.Mvc;
using ApiAudiencia.Models;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudController : ControllerBase
    {
        private readonly AudienciasContext _context;

        public SolicitudController(AudienciasContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Solicitud>> CrearSolicitud(Solicitud solicitud)
        {
            try
            {
                // Verifica si el empleado y proveedor existen en la base de datos
                var empleado = await _context.Empleados.FindAsync(solicitud.Empleado.DNI);
                var proveedor = await _context.Proveedores.FindAsync(solicitud.Proveedor.DNI);

                if (empleado == null || proveedor == null)
                {
                    return BadRequest("El empleado o proveedor no existen");
                }

                // Asigna los objetos empleado y proveedor a la solicitud
                solicitud.Empleado = empleado;
                solicitud.Proveedor = proveedor;

                // Agrega la solicitud a la base de datos
                _context.Solicitudes.Add(solicitud);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CrearSolicitud), new { id = solicitud.Nro }, solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
