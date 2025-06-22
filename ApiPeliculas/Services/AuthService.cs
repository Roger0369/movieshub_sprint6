// Importa namespaces necesarios para trabajar con bases de datos, criptografía y codificación.
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using ApiPeliculas.Models;

namespace ApiPeliculas.Services
{
    // Servicio de autenticación para validar credenciales de usuario.
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // Método asincrónico que valida las credenciales del usuario y devuelve el usuario si es válido.
        public async Task<User?> ValidateAndGetUserAsync(UserLogin login)
        {
            // Obtiene la cadena de conexión desde la configuración.
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Crea y abre una conexión a la base de datos SQL Server.
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Codifica la contraseña ingresada en bytes.
            var passwordBytes = Encoding.UTF8.GetBytes(login.Password);
            var hashedPassword = Convert.ToHexString(SHA512.HashData(passwordBytes));

            // Consulta SQL para verificar si existe un usuario con ese nombre y contraseña (hasheada).
            var query = "SELECT Id, Username FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";

            using var command = new SqlCommand(query, connection);

            // Asigna los valores de los parámetros a la consulta para evitar SQL Injection.
            command.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = login.Username;
            command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 128).Value = hashedPassword;

            // Ejecuta la consulta de forma asíncrona y lee el resultado.
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1)
                };
            }

            return null;
        }
    }
}


