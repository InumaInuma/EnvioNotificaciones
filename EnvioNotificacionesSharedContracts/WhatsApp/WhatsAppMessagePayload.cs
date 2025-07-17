using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    public class WhatsAppMessagePayload
    {
        [JsonPropertyName("messaging_product")] public string MessagingProduct { get; set; } = "whatsapp";
        [JsonPropertyName("recipient_type")] public string RecipientType { get; set; } = "individual";
        [JsonPropertyName("to")] public string To { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; } = "template";
        [JsonPropertyName("template")] public WhatsAppTemplate Template { get; set; } = null!;
    }
}
