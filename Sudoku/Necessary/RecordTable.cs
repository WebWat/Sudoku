using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Sudoku.Necessary
{
    internal static class RecordTable
    {
        public static List<RecordInformation> Data = new();
        private const string FILENAME = "info.json";

        public static void Read()
        {
            using (var reader = new FileStream(FILENAME, FileMode.OpenOrCreate))
            {
                Data.Concat(JsonSerializer.Deserialize<List<RecordInformation>>(reader) ?? new()).OrderBy(i => i.Number);
            }
        }

        public static void Save()
        {
            using (var writer = new FileStream(FILENAME, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(writer, Data);
            }
        }
    }
}
