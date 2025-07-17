using EnvioNotificacionesApplication.Service;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesDomian.Security;
using EnvioNotificacionesInfrastructure.Data;
using EnvioNotificacionesInfrastructure.Repositories;
using EnvioNotificacionesInfrastructure.Security;
using EnvioNotificacionesInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;
using Seguridad;
//using WhatsAppNotifierWorkerService;


namespace EnvioNotificacionesUIWinForms
{
    internal static class Program
    {
        private static IHost? _host;
        

        [STAThread]
        static async Task Main() // ¡Importante que s ea async Task Main!
        {
            ApplicationConfiguration.Initialize();

            _host = CreateHostBuilder().Build();

            await _host.StartAsync(); // Espera a que el host se inicie completamente

            using (var serviceScope = _host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var loginForm = services.GetRequiredService<Form1>();

                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        var mainForm = services.GetRequiredService<FormInicio>();
                        Application.Run(mainForm);
                    }
                    else
                    {
                        // Si el login falla o se cancela, detén el host
                        await _host.StopAsync();
                        _host.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Un error ocurrió al iniciar la aplicación: {ex.Message}\n\nDetalle: {ex.InnerException?.Message}", "Error de Inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Este código se ejecuta cuando el MainForm se cierra.
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
        }

        static IHostBuilder CreateHostBuilder()
        {
            Cifrado _cifrado = new Cifrado();
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    
                    
                    var conexion = _cifrado.Desencrit(connectionString);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(conexion));

                    services.AddScoped<ICitaRepository, CitaRepository>();
                    services.AddScoped<IUsuarioRepository, UsuarioRepository>();
                    services.AddTransient<IPasswordHasher, PasswordHasher>();
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<ICitaManagementService, CitaManagementService>();

                    // *** ¡LA CLAVE ESTÁ AQUÍ! REGISTRA IWhatsAppService con HttpClientFactory ***
                    // Esto registrará IWhatsAppService y, cuando se pida, HttpClientFactory
                    // se encargará de crear y proporcionar un HttpClient al constructor de WhatsAppService.
                    services.AddHttpClient<IWhatsAppService, WhatsAppService>(client =>
                    {
                        // Opcional: Puedes configurar aquí la base URL u otros settings de HttpClient
                        // client.BaseAddress = new Uri("http://your-whatsapp-api-url.com/");
                        // client.DefaultRequestHeaders.Add("Accept", "application/json");
                    });

                    // Registra tu Worker
                    //services.AddHostedService<Worker>();

                    // Registro de Formularios
                    services.AddTransient<Form1>();
                    services.AddTransient<FormInicio>();
                    services.AddTransient<FormObservadosPacientes>();// IMPORTANTE: Registra el formulario
                });
        }
    }
}