using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UI.Necessary
{
    internal static class RecordTable
    {
        private static List<RecordInformation> _data = new();
        private static bool _isRead = false;
        private const string FILENAME = "info.json";

        public static void Add(RecordInformation info)
        {
            _data.Add(info);
        }

        public static List<RecordInformation> GetList()
        {
            var newList = new List<RecordInformation>();

            for (int i = 0; i < _data.Count; i++)
            {
                var temp = _data[i];

                newList.Add(new RecordInformation
                {
                    DateTimeReceive = temp.DateTimeReceive,
                    Difficult = temp.Difficult,
                    Minutes = temp.Minutes,
                    Seconds = temp.Seconds
                });
            }

            return newList;
        }

        public static void Read()
        {
            if (!_isRead)
            {
                try
                {
                    using var reader = new FileStream(FILENAME, FileMode.OpenOrCreate);
                    _data.AddRange(JsonSerializer.Deserialize<List<RecordInformation>>(reader) ?? new());
                }
                catch (JsonException)
                {
                }
                finally
                {
                    _isRead = true;
                }
            }
        }

        public static void Save()
        {
            Read();
            using var writer = new FileStream(FILENAME, FileMode.OpenOrCreate);
            JsonSerializer.Serialize(writer, _data);
        }
    }
}
