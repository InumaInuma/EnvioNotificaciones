using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    public class WhatsAppComponent
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("parameters")] public List<WhatsAppParameter> Parameters { get; set; } = new List<WhatsAppParameter>();
    }
}
