using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.Configuraciones
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private readonly string _format = "HH:mm:ss";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            =>
            TimeOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            =>
            writer.WriteStringValue(value.ToString(_format));
    }
}
