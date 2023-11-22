using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UI.Necessary
{
    internal static class RecordTable
    {
        public static List<RecordInformation> Data = new();
        private static bool _isReaded = false;
        private const string FILENAME = "info.json";

        public static void Read()
        {
            if (!_isReaded)
            {
                try
                {
                    using var reader = new FileStream(FILENAME, FileMode.OpenOrCreate);

                    Data.AddRange(JsonSerializer.Deserialize<List<RecordInformation>>(reader) ?? new());
                }
                catch (JsonException)
                {
                }
                finally
                {
                    _isReaded = true;
                }
            }
        }

        public static void Save()
        {
            using var writer = new FileStream(FILENAME, FileMode.OpenOrCreate);
            writer.Position = 0;

            JsonSerializer.Serialize(writer, Data);
        }
    }
}
