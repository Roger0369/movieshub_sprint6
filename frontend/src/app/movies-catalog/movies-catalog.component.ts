import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 

// Modelo de datos de una película
export interface Movie {
  id: number;
  name: string;
  description: string;
  image: string;
  isFavorite: boolean;
  isHidden: boolean;
}

@Component({
  selector: 'app-movie-catalog', // Nombre del componente para su uso en HTML
  standalone: true, // Indica que este componente no depende de un módulo externo
  imports: [CommonModule, RouterModule], // Importa módulos necesarios para la vista
  templateUrl: './movies-catalog.component.html',
  styleUrls: ['./movies-catalog.component.css'],
})

// Clase del componente que maneja la lógica del catálogo de películas
export class MovieCatalogComponent {
  movies: Movie[] = [];
  showingFavorites = false; // Para indicar si se están mostrando solo favoritos
  notificationMessage: string | null = null; // Mensaje que se mostrará al usuario después de una acción
  movieService: any;

  // Inyección de dependencias: HttpClient para peticiones y Router para navegación
  constructor(private http: HttpClient, private router: Router) {
    this.loadMovies(); // Carga todas las películas al iniciar
  }

  // Método para cargar todas las películas desde el servidor
  loadMovies() {
    this.http.get<Movie[]>('http://localhost:5291/api/movies')
      .subscribe({
        next: (data) => {
          this.movies = data;
          this.showingFavorites = false;
        },
        error: (err) => {
          console.error('Error loading movies', err);
        }
      });
  }

  // Carga las películas que están marcadas como favoritas
  showFavorites() {
    this.http.get<Movie[]>('http://localhost:5291/api/movies/favorites')
      .subscribe({
        next: (data) => {
           console.log('Favoritos recibidos:', data); //
          this.movies = data;
          this.showingFavorites = true;
        },
        error: (err) => {
          console.error('Error loading favorites', err);
        }
      });
  }
// Alterna el estado de favorito de una película
toggleFavorite(id: number) {
  this.http.post(`http://localhost:5291/api/movies/toggle-favorite`, id, { // Envío del ID como cuerpo de la solicitud
    headers: { 'Content-Type': 'application/json' }
  }).subscribe({
    next: () => {
      if (this.showingFavorites) { // Si estamos viendo favoritos, recarga favoritos. Si no, recarga todas las películas.
        this.showFavorites();
      } else {
        this.loadMovies();
      }
    },
    error: (err) => {
      console.error('Error toggling favorite', err);
    }
  });
}
  // Oculta una película (si el usuario confirma)
  hideMovie(movieId: number) {
  if (confirm('¿Deseas ocultar esta película?')) {
    this.http.post(`http://localhost:5291/api/movies/hide`, { id: movieId })
      .subscribe({
        next: () => {
          this.notificationMessage = `La película ha sido ocultada`;
          this.loadMovies(); // Refresca la lista
          setTimeout(() => { // Oculta el mensaje después de 2 segundos
            this.notificationMessage = null;
          }, 2000); 
        }, 
        error: (err) => {
          console.error('Error hiding movie', err);
        }
      });
  }
}

  // Restaura todas las películas ocultas
  restoreHiddenMovies() {
  this.http.post('http://localhost:5291/api/movies/restore-hidden', {})
    .subscribe({
      next: (response: any) => {
        this.notificationMessage = response.message;
        this.loadMovies(); // Recarga el catálogo completo
        setTimeout(() => {
          this.notificationMessage = null;
        }, 2000);
      },
      error: (err) => {
        console.error('Error restoring movies', err);
      }
    });
}

// Método para navegar a la ruta de detalle de una película específica
seeDetails(movie: Movie) {
  this.router.navigate(['/pelicula', movie.id]);
}
}
