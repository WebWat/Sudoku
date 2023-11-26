using SudokuLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UI.Necessary;

namespace UI
{
    public partial class RecordTableWindow : Window
    {
        public class Info
        {
            public string DateTime { get; set; } = default!;
            public string SolutionTime { get; set; } = default!;
            public string Difficult { get; set; } = default!;
        }

        public List<Info> Table;

        public RecordTableWindow()
        {
            InitializeComponent();

            DataContext = this;

            Table = new();
            RecordTable.Read();
            var group = RecordTable.GetList().GroupBy(i => i.Difficult).ToList();
            var result = new List<RecordInformation>();

            RecordInformation temp;

            group.ForEach(element =>
            {
                result.AddRange(element.OrderBy(time => time.Minutes * 60 + time.Seconds).Take(10));
            });

            Table = result.OrderBy(time => time.Minutes * 60 + time.Seconds)
                    .Select(item =>
                    {
                        return new Info
                        {
                            DateTime = item.DateTimeReceive.ToString("dd.MM.yyyy HH:mm:ss"),
                            SolutionTime = $"{item.Minutes:d2}:{item.Seconds:d2}",
                            Difficult = NameOfDifficult(item.Difficult)
                        };
                    }).ToList();

            ListView.ItemsSource = Table;
        }

        private string NameOfDifficult(Difficult difficult) => difficult switch
        {
            Difficult.Hard => "Сложный",
            Difficult.Medium => "Средний",
            Difficult.Easy => "Легкий",
            Difficult.Dev => "Экспериментальный",
            _ => ""
        };
    }
}
