// Define el espacio de nombres donde se encuentra la clase User
namespace ApiPeliculas.Models
{
    // Clase que representa la entidad User en la base de datos
    public class User
    {
        // Identificador único del usuario (clave primaria en la base de datos)
        public int Id { get; set; }

        // Nombre de usuario (debe ser único en la base de datos)
        public string Username { get; set; } = string.Empty;

        // Contraseña del usuario, almacenada como un hash en formato texto (por seguridad)
        public string PasswordHash { get; set; } = string.Empty;
    }
}

