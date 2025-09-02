using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiAudiencia.Custom;
using ApiAudiencia.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true);

var CorsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        policy.WithOrigins(
            "http://localhost:8080", // Para desarrollo local
            "https://audiencia-l864aoahc-lautarosanche-gmailcoms-projects.vercel.app" // Frontend en Vercel
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

    // Solo en desarrollo, permitir cualquier origen temporalmente
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    }
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireClaim("EsAdmin", "true"));
});


builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]!))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AudienciasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<Utilidades>();
// Configuración SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

 app.UseSwagger();
  app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();

// Aplicar CORS
app.UseCors(builder.Environment.IsDevelopment() ? "AllowAll" : CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API FUNCIONANDO...");
app.Run(); // Escucha en el puerto 80 dentro del contenedor