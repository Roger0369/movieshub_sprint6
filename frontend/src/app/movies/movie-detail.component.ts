import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 
import { Movie } from '../models/movie.models'; // Importación de la interfaz Movie desde la carpeta de modelos

@Component({ // Decorador que define el componente
  selector: 'app-movie-detail', // Nombre del selector del componente
  standalone: true, // Componente independiente (no requiere un módulo)
  imports: [CommonModule, RouterModule], // Módulos necesarios para la vista
  templateUrl: './movie-detail.component.html', // Plantilla HTML asociada
  styleUrls: ['./movie-detail.component.css'] // Estilos del componente
})

// Propiedad que almacenará la película actual (puede ser undefined al principio)
export class MovieDetailComponent {
  movie?: Movie;
  suggestions: Movie[] = [];

// Constructor con inyección de dependencias: ruta actual, router y HTTP
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {
    this.route.params.subscribe(params => { // Se suscribe a los parámetros de la ruta para obtener el ID de la película
      const id = +params['id']; // Se convierte a número con +
      this.loadMovie(id); // Carga los detalles de la película
    });
  }

  // Método para cargar los detalles de una película por su ID
  // Utiliza HttpClient para hacer una petición GET al servidor
  loadMovie(id: number) {
    this.http.get<Movie>(`http://localhost:5291/api/movies/${id}`)
      .subscribe({
        next: (data) => {
          this.movie = data;  // Asigna la película al componente
          this.loadSuggestions(data.id); // Carga sugerencias 
        },
        error: (err) => {
          console.error('Error cargando detalles de película', err);
        }
      });
  }

  // Método para cargar películas sugeridas (excluye la actual)
  loadSuggestions(currentId: number) {
    this.http.get<Movie[]>('http://localhost:5291/api/movies')
      .subscribe({
        next: (movies) => {
          // Filtra las películas para eliminar la actual del listado
          this.suggestions = movies.filter(m => m.id !== currentId);
        },
        error: (err) => {
          console.error('Error cargando sugerencias', err);
        }
      });
  }
  // Método para navegar a los detalles de otra película
  // Utiliza el router para cambiar la ruta a la página de detalles de la película seleccionada
  seeDetails(movie: Movie) {
    this.router.navigate(['/pelicula', movie.id]);
  }

  // Método para volver al catálogo principal
    goToCatalog() {
    this.router.navigate(['/movie-catalog']);
  }
}
