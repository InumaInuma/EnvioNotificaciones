using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    public class WhatsAppLanguage
    {
        [JsonPropertyName("code")] public string Code { get; set; } = "es";
    }

}
