import { Routes } from '@angular/router'; // Importa el tipo Routes, que define la estructura de las rutas en Angular.
import { LoginComponent } from './login/login.component'; // Importa el componente de login que se mostrará en la ruta principal ('/').
import { MovieCatalogComponent } from './movies-catalog/movies-catalog.component'; // Importa el componente que muestra el catálogo de películas.

export const routes: Routes = [ // Define el arreglo de rutas de la aplicación. Cada objeto representa una ruta.
  { path: '', component: LoginComponent }, // Ruta por defecto ('') que redirige al componente de login.
  { path: 'movie-catalog', component: MovieCatalogComponent }, // Ruta para mostrar el catálogo de películas.
  
  // Ruta dinámica para mostrar el detalle de una película específica.
  // Se usa carga perezosa (lazy load) con loadComponent para mejorar el rendimiento.
  {
    path: 'pelicula/:id', // ':id' es un parámetro dinámico que representa el ID de la película.
    loadComponent: () =>
      import('./movies/movie-detail.component') // Importa el componente solo cuando se necesita.
      .then(m => m.MovieDetailComponent) // Extrae el componente desde el módulo importado.
  }
];
