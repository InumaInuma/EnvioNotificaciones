using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnvioNotificacionesSharedContracts.WhatsApp
{
    public class WhatsAppParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        // ¡AQUÍ ESTÁ LA CLAVE! El nombre de la propiedad en C# es PascalCase
        [JsonPropertyName("parameter_name")]
        public string ParameterName { get; set; } = null!; // Antes: parameter_name

        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;
    }
}
