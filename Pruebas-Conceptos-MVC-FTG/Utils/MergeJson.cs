using System.Text.Json;

namespace Pruebas_Conceptos_MVC_FTG.Utils
{
    public static class JsonUtils
    {
        public static JsonElement MergeJson(JsonElement original, JsonElement updates)
        {
            var dicOriginal = JsonSerializer.Deserialize<Dictionary<string, object>>(original.GetRawText())!;
            var dicUpdates = JsonSerializer.Deserialize<Dictionary<string, object>>(updates.GetRawText())!;

            foreach (var kvp in dicUpdates)
            {
                dicOriginal[kvp.Key] = kvp.Value;
            }

            var jsonMerged = JsonSerializer.Serialize(dicOriginal);
            return JsonDocument.Parse(jsonMerged).RootElement;
        }
    }
}
