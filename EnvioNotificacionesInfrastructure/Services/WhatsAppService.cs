using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesSharedContracts.WhatsApp;
using Microsoft.Extensions.Configuration;
using Seguridad;

namespace EnvioNotificacionesInfrastructure.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        Cifrado _cifrado = new Cifrado();   
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _apiToken;

        public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = "https://graph.facebook.com/v22.0/660863010439433/messages";

            // Obtén el token de forma segura desde la configuración
            _apiToken = configuration["WhatsAppApi:AccessToken"] ?? throw new InvalidOperationException("WhatsApp API Access Token no configurado en appsettings.json.");
           
            var apitoken = _cifrado.Desencrit(_apiToken);    
            // Es mejor configurar el Authorization header cuando se registra el HttpClient
            // vía HttpClientFactory en Program.cs de los proyectos host.
            // Para asegurar que esté presente aquí, podemos añadirlo defensivamente:
            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apitoken}");
            }
        }

        private async Task<bool> SendWhatsAppMessageAsync(WhatsAppMessagePayload payload)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(payload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"[WhatsAppService] Enviando mensaje a {payload.To}: {json}");
                var response = await _httpClient.PostAsync(_apiBaseUrl, content);

                // No uses response.EnsureSuccessStatusCode() directamente si quieres capturar el cuerpo del error
                // si el status code no es 2xx.
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Si no fue exitoso, loguea el error y lanza una excepción con detalles
                    Console.Error.WriteLine($"[WhatsAppService] Fallo en el envío a {payload.To}. Código: {response.StatusCode}. Respuesta: {responseContent}");
                    throw new HttpRequestException($"Fallo en el envío a WhatsApp: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {responseContent}", null, response.StatusCode);
                }

                var apiResponse = JsonSerializer.Deserialize<WhatsAppApiResponse>(responseContent);
                Console.WriteLine($"[WhatsAppService] Mensaje enviado. ID: {apiResponse?.Messages?.FirstOrDefault()?.Id ?? "N/A"}");
                return true;
            }
            catch (HttpRequestException httpEx)
            {
                Console.Error.WriteLine($"[WhatsAppService] Error de HTTP/red al enviar mensaje: {httpEx.Message}.");
                throw; // Relanza la excepción para que sea manejada por la capa superior (Worker Service)
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WhatsAppService] Error inesperado al enviar mensaje: {ex.Message}.");
                throw;
            }
        }

        public async Task<bool> EnviarBienvenidaAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string tipoExamen)
        {
            var payload = new WhatsAppMessagePayload
            {
                To = numeroDestino,
                Template = new WhatsAppTemplate
                {
                    Name = "medcorpx_generacion_de_cita",
                    
                    Components = new List<WhatsAppComponent>
                    {
                        new WhatsAppComponent
                        {
                            Type = "body",
                            Parameters = new List<WhatsAppParameter>
                            {
                                new WhatsAppParameter { Type = "text", ParameterName = "nombre_paciente", Text = nombrePaciente },
                                new WhatsAppParameter { Type = "text", ParameterName = "fecha", Text = fechaCita.ToString("dd/MM/yyyy") },
                                new WhatsAppParameter { Type = "text", ParameterName = "tipo_examen", Text = tipoExamen }
                            }
                        }
                    }
                }
            };
            return await SendWhatsAppMessageAsync(payload);
        }

        public async Task<bool> EnviarRecordatorioAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string tipoExamen)
        {
            var payload = new WhatsAppMessagePayload
            {
                To = numeroDestino,
                Template = new WhatsAppTemplate
                {
                    Name = "medcorpx_recordatorio_de_cita",
                    Components = new List<WhatsAppComponent>
                    {
                        new WhatsAppComponent
                        {
                            Type = "body",
                            Parameters = new List<WhatsAppParameter>
                            {
                                new WhatsAppParameter { Type = "text", ParameterName = "nombre_paciente", Text = nombrePaciente },
                                new WhatsAppParameter { Type = "text", ParameterName = "fecha", Text = fechaCita.ToString("dd/MM/yyyy") },
                                new WhatsAppParameter { Type = "text", ParameterName = "tipo_examen", Text = tipoExamen }
                            }
                        }
                    }
                }
            };
            return await SendWhatsAppMessageAsync(payload);
        }



        public async Task<bool> EnviarFinalizadoAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string empresa, string interconsultas, string correo)
        {
            var payload = new WhatsAppMessagePayload
            {
                To = numeroDestino,
                Template = new WhatsAppTemplate
                {
                    Name = "medcorpx_interconsulta",
                    Components = new List<WhatsAppComponent>
            {
                new WhatsAppComponent
                {
                    Type = "body",
                    Parameters = new List<WhatsAppParameter>
                    {
                        // ¡Aquí es donde debes corregir! Usa ParameterName (con P y N mayúsculas)
                        new WhatsAppParameter { Type = "text", ParameterName = "nombre_paciente", Text = nombrePaciente },
                        new WhatsAppParameter { Type = "text", ParameterName = "fecha", Text = fechaCita.ToString("dd/MM/yyyy") },
                        new WhatsAppParameter { Type = "text", ParameterName = "empresa", Text = empresa },
                        new WhatsAppParameter { Type = "text", ParameterName = "interconsultas", Text = interconsultas },
                        new WhatsAppParameter { Type = "text", ParameterName = "correo", Text = correo }
                    }
                }
            }
                }
            };
            return await SendWhatsAppMessageAsync(payload);
        }

    }
}