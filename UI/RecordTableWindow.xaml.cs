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
            RecordTable.Data.OrderBy(i => i.Minutes * i.Seconds);

            RecordInformation temp;

            for (int i = 0; i < RecordTable.Data.Count; i++)
            {
                temp = RecordTable.Data[i];

                Table.Add(new Info
                {
                    DateTime = temp.DateTimeReceive.ToString("dd.MM.yyyy HH:mm:ss"),
                    SolutionTime = $"{temp.Minutes}:{temp.Seconds}",
                    Difficult = NameOfDifficult(temp.Difficult)
                });
            }

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
