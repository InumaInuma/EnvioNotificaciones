using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    public class WhatsAppTemplate
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("language")] public WhatsAppLanguage Language { get; set; } = new WhatsAppLanguage { Code = "es" };
        [JsonPropertyName("components")] public List<WhatsAppComponent> Components { get; set; } = new List<WhatsAppComponent>();
    }
}
