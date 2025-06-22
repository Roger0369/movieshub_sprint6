using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiPeliculas.Models;
using ApiPeliculas.Services;

namespace ApiPeliculas.Controllers

// Controlador de API
{
    [ApiController]
    [Route("api/[controller]")] // Define la ruta base como "api/auth"
    public class AuthController : ControllerBase
    {
        // Servicios inyectados
        private readonly AuthService _authService; // AuthService: maneja la lógica de autenticación y validación de usuarios
        private readonly IConfiguration _configuration; // IConfiguration: permite acceder a la configuración de la aplicación (como claves y URLs)

        // Constructor: se inyectan el servicio de autenticación y la configuración
        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        // Endpoint POST: api/auth/login
        // Recibe un objeto UserLogin con las credenciales del usuario
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            var user = await _authService.ValidateAndGetUserAsync(login); // Valida el usuario y contraseña usando el AuthService

            if (user == null) // Si no se encuentra el usuario o las credenciales son inválidas, devuelve 401
                return Unauthorized("Credenciales inválidas");

            var token = GenerateJwtToken(user.Username); // Si es válido, genera un token JWT
            return Ok(new { token }); // Devuelve el token como respuesta
        }
        // Método privado para generar un JWT a partir del nombre de usuario
        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));  // Se obtiene la clave secreta desde la configuración (appsettings.json)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Se crean las credenciales de firma usando HMAC SHA256

            var claims = new[] // Lista de claims que se incluirán en el token (en este caso, solo el nombre de usuario)

            {
                new Claim(ClaimTypes.Name, username)
            };
            // Se crea el token JWT con los claims, emisor, audiencia, expiración y firma
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Duración del token (1 hora)
                signingCredentials: creds
            );
            // Se serializa el token para enviarlo como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
