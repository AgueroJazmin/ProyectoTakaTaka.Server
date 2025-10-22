using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoTakaTaka.Shared.Configuraciones
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            =>
            DateOnly.ParseExact(reader.GetString()!, _format, null);

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            =>
            writer.WriteStringValue(value.ToString(_format));
    }
}
