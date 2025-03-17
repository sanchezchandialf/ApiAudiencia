using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiAudiencia.Custom;
using ApiAudiencia.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.secret.json", optional: false, reloadOnChange: true);

var CorsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173", // Para desarrollo local
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));

builder.Services.AddSingleton<Utilidades>();

var app = builder.Build();

// Pipeline middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Aplicar CORS
app.UseCors(builder.Environment.IsDevelopment() ? "AllowAll" : CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
