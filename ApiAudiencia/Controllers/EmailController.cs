using System.Security.Claims;
using ApiAudiencia.Models;
using ApiAudiencia.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;
    private readonly AudienciasContext _context;

    public EmailController(IEmailService emailService, ILogger<EmailController> logger, AudienciasContext context)
    {
        _emailService = emailService;
        _logger = logger;
        _context = context;
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Auth([FromBody] EmailRequestDTO request)
    {
    if (string.IsNullOrWhiteSpace(request.Destinatario))
    {
        return BadRequest(new { Success = false, Message = "El correo electrónico es requerido" });
    }

    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Destinatario);
    
    if (usuario == null)
    {
        return Ok(new { Success = false, Message = "El correo no esta registrado" });
    }

    try
    {
        // Generar código de 6 dígitos
        var random = new Random();
        var emailrequest = new EmailRequest
        {
            // Guardar el código en la base de datos (con fecha de expiración)
            CodigoRecuperacion = random.Next(100000, 999999).ToString(),
            CodigoRecuperacionExpira = DateTime.UtcNow.AddMinutes(15), // Expira en 15 minutos
            Destinatario = request.Destinatario
        };
        
        _context.EmailRequests.Add(emailrequest);
        await _context.SaveChangesAsync();

        // Preparar contenido del email con el código real
        var asunto =  $"Código de Autenticación {DateTime.Today}";
        var contenido = $@"<!DOCTYPE html>
                        <html>
                        <head>
                        <meta charset=""UTF-8"">
                        <title>Recuperación de contraseña</title>
                        </head>
                        <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
                        <table width=""100%"" style=""max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px;"">
                        <tr>
                        <td>
                        <h2 style=""color: #333333;"">Recuperación de contraseña</h2>
                        <p>Hola,</p>
                        <p>Recibimos una solicitud para restablecer tu contraseña en la aplicación de gestión de audiencia.</p>
                        <p>Usá el siguiente código para completar el proceso:</p>
                        <p style=""font-size: 24px; font-weight: bold; color: #d02d69;"">{emailrequest.CodigoRecuperacion}</p>
                        <p>Ingresá este código en el formulario de recuperación para continuar.</p>
                        <p>Este código expirará en 15 minutos.</p>
                        <p>Si no solicitaste esto, podés ignorar este mensaje.</p>
                        <p style=""margin-top: 30px;"">Saludos,<br>Equipo de Soporte</p>
                        </td>
                        </tr>
                        </table>
                        </body>
                        </html>";
        await _emailService.SendEmailAsync(request.Destinatario, asunto, contenido);
        //return Ok(new { Success = true, Message = "Si el correo existe, se ha enviado un código de verificación" });
        return Ok(emailrequest);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al enviar correo");
        return StatusCode(500, new { Success = false, Message = "Error al procesar la solicitud", Error = ex.Message });
    }
}
}