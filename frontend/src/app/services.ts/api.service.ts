// src/app/services/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie } from '../models/movie.models';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5291/api';

  constructor(private http: HttpClient) {}

  // Obtener todas las películas
  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/movies`);
  }

  // Agregar a favoritos
  addToFavorites(movieId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/favorites`, { movieId });
  }

  // Obtener favoritos
  getFavorites(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/favorites`);
  }

  // Eliminar de favoritos
  removeFromFavorites(movieId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/favorites/${movieId}`);
  }

  // Ocultar película
  hideMovie(movieId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/movies/${movieId}/hide`, {});
  }

  // Restaurar películas ocultas
  restoreHiddenMovies(): Observable<any> {
    return this.http.post(`${this.baseUrl}/movies/restore-hidden`, {});
  }

  // Obtener películas visibles (si las ocultas se excluyen)
  getVisibleMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/movies/visible`);
  }
}

