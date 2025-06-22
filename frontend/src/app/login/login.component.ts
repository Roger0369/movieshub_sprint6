// Importa los decoradores y servicios necesarios de Angular
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // Para hacer peticiones HTTP
import { Router } from '@angular/router'; // Para navegar entre rutas
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Necesario para usar ngModel

@Component({
  selector: 'app-login', // Nombre del selector del componente
  standalone: true, // Componente independiente (sin necesidad de un módulo)
  imports: [CommonModule, FormsModule], // Módulos necesarios para el funcionamiento del HTML
  templateUrl: './login.component.html', // Ruta de la plantilla HTML
  styleUrls: ['./login.component.css'] // Estilo asociado al componente
})
export class LoginComponent {
  // Variables enlazadas al formulario
  username: string = '';
  password: string = '';
  message: string = '';

  // Inyecta HttpClient para hacer peticiones HTTP y Router para navegar entre rutas
  // El constructor inicializa las dependencias necesarias para el component
  constructor(private http: HttpClient, private router: Router) {}

  // Método que se ejecuta al enviar el formulario de login
  onSubmit() {
  this.http.post<any>('http://localhost:5291/api/Auth/login', { // Realiza una petición POST al servidor para autenticar al usuario
    username: this.username,
    password: this.password
  }).subscribe({
      next: res => {  // Si la respuesta es exitosa y contiene un token
    if (res.token) {
      this.router.navigate(['/movie-catalog']); // Redirige al catálogo de películas
    } else { // Si no hay token, muestra mensaje de fallo
      this.message = 'Login fallido.';
    }
  },
      error: err => { // Si ocurre un error durante la petición
        if (err.status === 401) {
          this.message = 'Usuario o contraseña incorrectos.';
        } else {
          this.message = 'Error del servidor.';
        }
      }
    });
  }
}

