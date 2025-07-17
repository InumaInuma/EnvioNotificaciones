using EnvioNotificacionesApplication.Service;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.Data;
using EnvioNotificacionesInfrastructure.Repositories;
using EnvioNotificacionesInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WhatsAppNotifierWorkerService;
using Microsoft.Extensions.Hosting.WindowsServices;
using Seguridad; // <-- ¡Añade este using para AddWindowsService!


Cifrado _cifrado = new Cifrado();
// 1. Configuración de appsettings.json
// El HostApplicationBuilder ya carga appsettings.json por defecto.
// Puedes acceder a la configuración a través de builder.Configuration
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// 2.0. Configuración para ejecutar como Servicio de Windows (¡CRUCIAL para producción!)
// Si tu Worker va a correr como un servicio de Windows, ¡descomenta esta línea!
// Esto añade la integración necesaria para que el sistema operativo lo gestione.
//builder.Services.AddWindowsService(); // <-- ¡Descomentar si es un servicio de Windows!
// 2. Configuración de Servicios (Inyección de Dependencias)
builder.Services.AddHostedService<Worker>(); // Registra tu Worker principal
builder.Services.AddWindowsService();
// 2.1. Configuración de DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//var cone = _cifrado.Encrit(connectionString);
var conexion = _cifrado.Desencrit(connectionString);   

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(conexion)
// Opcional: Habilitar logging de Entity Framework Core para depuración
//.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
//.EnableSensitiveDataLogging() // Solo para desarrollo, no en producción
);

// 2.2. Configuración de HttpClient para WhatsAppService (¡Muy importante!)
// Usa HttpClientFactory para una gestión eficiente de los HttpClient
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>(client =>
{
    // Puedes configurar headers por defecto aquí si lo deseas, aunque ya lo haces en el constructor del servicio.
    // client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// 2.3. Registro de Repositorios (Interfaces y sus Implementaciones)
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<ICitaManagementService, CitaManagementService>(); // <-- ¡Esta es la que faltaba!
// Registra otros repositorios si los creas

// 2.4. Registro del Servicio de WhatsApp
// Ya está registrado arriba con AddHttpClient<IWhatsAppService, WhatsAppService>


// 3. Opcional: Configuración de Logging
// builder.Logging.AddConsole(); // Ejemplo básico, puedes usar Serilog, NLog, etc.
builder.Logging.SetMinimumLevel(LogLevel.Information); // O LogLevel.Warning, LogLevel.Error

IHost host = builder.Build();
host.Run();
