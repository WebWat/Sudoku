using Sudoku.Necessary;
using SudokuLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Логика взаимодействия для RecordTableWindow.xaml
    /// </summary>
    public partial class RecordTableWindow : Window
    {
        public class Info
        {
            public string DateTime { get; set; }
            public string SolutionTime { get; set; }
            public string Difficult { get; set; }
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
