using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPeliculas.Data;
using ApiPeliculas.Models;

namespace ApiPeliculas.Controllers

{
    // Indica que esta clase es un controlador de API y que sus rutas comienzan con "api/movies".
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Inyección del contexto de base de datos
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Método GET: api/movies
        // Devuelve una lista de todas las películas que no están ocultas.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
        {
            return await _context.Movies
                .Where(m => m.IsHidden == false || m.IsHidden == null)
                .ToListAsync();
        }

        // Método GET: api/movies/{id}
        // Busca una película por su ID y la devuelve si existe.
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
            return movie;
        }

        // Método POST: api/movies
        // Crea una nueva película y la guarda en la base de datos.
        [HttpPost]
        public async Task<ActionResult<Movie>> Create(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie); // Devuelve un código 201 Created con la ubicación de la película creada
        }
        // Método PUT: api/movies/{id}
        // Actualiza los datos de una película existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Movie updatedMovie)
        {
            if (id != updatedMovie.Id) // Verifica que el ID de la URL coincida con el del objeto
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo.");
            }

            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound("Película no encontrada.");
            }

            // Actualiza los campos de la película existente con los nuevos valores
            existingMovie.Name = updatedMovie.Name;
            existingMovie.Description = updatedMovie.Description;

            // Marca la entidad como modificada y guarda cambios.
            _context.Movies.Update(existingMovie);
            await _context.SaveChangesAsync();

            // Devuelve 204 NoContent para indicar éxito sin contenido.
            return NoContent();
        }

        // Método POST: api/movies/toggle-favorite
        [HttpPost("toggle-favorite")] // Alterna el estado de favorito de una película
        public async Task<IActionResult> ToggleFavorite([FromBody] int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                return NotFound("Película no encontrada.");
            }

            movie.IsFavorite = !movie.IsFavorite;
            await _context.SaveChangesAsync();

            return Ok(new { movie.Id, movie.IsFavorite });
        }

        // Método GET: api/movies/favorites
        [HttpGet("favorites")] // Devuelve una lista de todas las películas marcadas como favoritas
        public async Task<ActionResult<IEnumerable<Movie>>> GetFavorites()
        {
            var favorites = await _context.Movies
                .Where(m => m.IsFavorite == true) // esto SÍ es compatible con EF
                .ToListAsync();

            return Ok(favorites);
        }

        // Método POST: api/movies/hide
        [HttpPost("hide")] // Oculta una película estableciendo IsHidden = true
        public async Task<IActionResult> HideMovie([FromBody] MovieIdDto dto)
        {
            if (dto == null || dto.Id == 0)
                return BadRequest("ID inválido.");

            var movie = await _context.Movies.FindAsync(dto.Id);
            if (movie == null)
                return NotFound("Película no encontrada.");

            movie.IsHidden = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Película con ID {dto.Id} fue ocultada." });
        }

        // Método POST: api/movies/restore-hidden
        [HttpPost("restore-hidden")] // Restaura todas las películas ocultas (IsHidden = false)
        public async Task<IActionResult> RestoreHiddenMovies()
        {
            var hiddenMovies = await _context.Movies
                .Where(m => m.IsHidden == true)
                .ToListAsync();

            if (!hiddenMovies.Any())
            {
                return NotFound("No hay películas ocultas para restaurar.");
            }

            foreach (var movie in hiddenMovies) // Recorre cada película y cambia su estado
            {
                movie.IsHidden = false;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Películas ocultas restauradas." });
        }

    }
}
