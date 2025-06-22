using Microsoft.AspNetCore.Authentication.JwtBearer; // Para autenticación con JWT
using Microsoft.EntityFrameworkCore; // Para trabajar con Entity Framework Core
using Microsoft.IdentityModel.Tokens; // Para validación de tokens JWT
using Microsoft.OpenApi.Models; // Para configuración de Swagger
using System.Text; // Para codificación de la clave JWT
using ApiPeliculas.Data; // Importa el contexto de base de datos
using ApiPeliculas.Services; // Importa servicios personalizados como AuthService

var builder = WebApplication.CreateBuilder(args); // Crea una aplicación web con configuración por defecto

// 1. Conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registro de servicios personalizados
builder.Services.AddScoped<AuthService>(); // Aquí se registra AuthService como servicio con ciclo de vida Scoped

// 3. Autenticación JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!); // Obtiene y codifica la clave secreta
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false, 
            ValidateIssuerSigningKey = true, // Valida la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(key) // Clave usada para validar la firma
        };
    });

// 4. Registro de controladores y Swagger (documentación de la API)
builder.Services.AddControllers(); // Habilita el uso de controladores
builder.Services.AddEndpointsApiExplorer(); // Descubre endpoints de la API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiPeliculas", Version = "v1" });

    // Configurar JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter 'Bearer' [space] and then your token in the text input below.  
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()  // Requiere token JWT para acceder a los endpoints protegidos desde Swagger
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// 5. Configura CORS para permitir solicitudes desde cualquier origen (útil en desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); // Permite todo tipo de origen, método y encabezado
    });
});

// Construye la aplicación
var app = builder.Build();

// 6. Middlewares
if (app.Environment.IsDevelopment()) // En entorno de desarrollo, habilita Swagger y su UI
{
    app.UseSwagger(); // Habilita generación de documentación Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculas v1"); // Ruta al JSON de Swagger
    });
}

app.UseCors("AllowAll"); // Habilita CORS con la política definida

app.UseAuthentication(); // Habilita autenticación y autorización en el pipeline
app.UseAuthorization();

app.MapControllers(); // Mapea los controladores (API endpoints)
app.Run(); // Ejecuta la aplicación