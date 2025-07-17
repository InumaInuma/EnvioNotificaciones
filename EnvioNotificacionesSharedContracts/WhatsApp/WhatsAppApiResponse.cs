using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    // Opcional: Clase para la respuesta de la API de WhatsApp, si quieres capturar el message_id
    public class WhatsAppApiResponse
    {
        [JsonPropertyName("messaging_product")] public string MessagingProduct { get; set; } = null!;
        [JsonPropertyName("messages")] public List<WhatsAppApiMessage> Messages { get; set; } = new List<WhatsAppApiMessage>();
    }

    public class WhatsAppApiMessage
    {
        [JsonPropertyName("id")] public string Id { get; set; } = null!;
    }
}
