using GestionContactosApi.Services;
using GestionContactosApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using GestionContactosApi.Configuration;


var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<SupabaseSettings>(builder.Configuration.GetSection("Supabase"));
builder.Services.AddHttpClient<ContactoService>();

// Configurar y registrar JwtSettings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>() 
    ?? throw new InvalidOperationException("La sección JwtSettings no se encuentra en appsettings.json");

// Servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<ContactoService>();
builder.Services.AddSingleton<AuthService>();

// Configurar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Endpoint de login
app.MapPost("/api/auth/login", (Usuario login, AuthService authService, IConfiguration config) =>
{
    var usuario = authService.Login(login.UserName, login.Password);
    if (usuario == null)
        return Results.Unauthorized();

    var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>() 
        ?? throw new InvalidOperationException("JwtSettings faltante");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, usuario.UserName),
        new Claim(ClaimTypes.Role, usuario.Rol)
    };

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: jwtSettings.Issuer,
        audience: jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes),
        signingCredentials: creds
    );

    var tokenStr = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new { token = tokenStr });
});

app.Run();


