using Microsoft.EntityFrameworkCore; // Importa el paquete de Entity Framework Core para trabajar con bases de datos
using ApiPeliculas.Models; // Importa los modelos de la aplicación

namespace ApiPeliculas.Data
{
    // Esta clase representa el contexto de base de datos para Entity Framework
    // Hereda de DbContext, lo que permite trabajar con la base de datos como objetos .NET
    public class ApplicationDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración del contexto (por ejemplo, la cadena de conexión)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Llama al constructor base de DbContext
        {
        }

        // Tabla 'Users'
        public DbSet<User> Users { get; set; }

        // Tabla 'Movies'
        public DbSet<Movie> Movies { get; set; }
    }
}