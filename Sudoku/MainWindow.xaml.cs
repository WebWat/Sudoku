using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, UICell> _cellsData = new Dictionary<int, UICell>();
        private Dictionary<int, Border> _borders = new Dictionary<int, Border>();

        private const int _rowOffset = 2;
        private const int _columnOffset = 1;

        private Solver solver;
        private bool _noteModeEnabled = false;
        private bool _errorPreventionModeEnabled = false;

        public MainWindow()
        {
            InitializeComponent();

            solver = new Solver(Solver.Generate(Difficult.Easy));

            for (int i = 0; i < SUDOKU_GRID.BOX_SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.BOX_SIZE; j++)
                {
                    CreateBox(i + _rowOffset, j + _columnOffset);
                }
            }

            solver.TrySolve();
        }

        private void CreateBox(int row, int column)
        {
            var boxBorder = new Border();

            boxBorder.BorderBrush = Brushes.Black;
            boxBorder.SetValue(Grid.RowProperty, row);
            boxBorder.SetValue(Grid.ColumnProperty, column);

            var uniformGrid = new UniformGrid();

            uniformGrid.Rows = SUDOKU_GRID.BOX_SIZE;
            uniformGrid.Columns = SUDOKU_GRID.BOX_SIZE;

            var cellXOffsite = (row - _rowOffset) * SUDOKU_GRID.BOX_SIZE;
            var cellYOffsite = (column - _columnOffset) * SUDOKU_GRID.BOX_SIZE;

            for (int i = 0; i < SUDOKU_GRID.BOX_SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.BOX_SIZE; j++)
                {
                    uniformGrid.Children.Add(CreateCell(i + cellXOffsite, j + cellYOffsite));
                }
            }

            boxBorder.Child = uniformGrid;

            BaseGrid.Children.Add(boxBorder);
        }

        private Grid CreateCell(int row, int column)
        {
            var container = new Grid();
            var border = new Border();
            var textBox = new TextBox();
            Label[] labels = default;

            if (solver[row, column].ToString() != string.Empty)
            {
                textBox.Text = solver[row, column].ToString();
            }
            else
            {
                var labelsGrid = new UniformGrid();
                labels = new Label[SUDOKU_GRID.SIZE];

                for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
                {
                    var label = new Label();

                    labels[i] = label;
                    labelsGrid.Children.Add(label);
                }

                border.Child = labelsGrid;
            }

            textBox.IsReadOnly = true;
            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;

            container.Children.Add(border);
            container.Children.Add(textBox);

            _borders.Add(textBox.GetHashCode(), border);
            _cellsData.Add(textBox.GetHashCode(), new UICell 
            { 
                Row = row,
                Column = column,
                TextBox = textBox,
                Labels = labels
            });

            return container;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var data = _cellsData[((TextBox)sender).GetHashCode()];
            var answer = solver[data.Row, data.Column].ToString();

            if (data.Labels == null || !int.TryParse(e.Text, out _))
            {
                return;
            }

            if (_errorPreventionModeEnabled)
            {
                if (e.Text != answer)
                {
                    var rowError = _cellsData.Values
                        .FirstOrDefault(i => i.Row == data.Row && i.TextBox.Text == answer);

                    var columnError = _cellsData.Values
                        .FirstOrDefault(i => i.Column == data.Column && i.TextBox.Text == answer);

                    var rowInc = data.Row - (data.Row % SUDOKU_GRID.BOX_SIZE);
                    var columnInc = data.Column - (data.Column % SUDOKU_GRID.BOX_SIZE);
                    var boxError = _cellsData.Values
                        .FirstOrDefault(i => 
                        i.Row <= data.Row &&
                        i.Row + rowInc + SUDOKU_GRID.BOX_SIZE >= data.Row &&
                        i.Column + columnInc <= data.Column &&
                        i.Column + columnInc + SUDOKU_GRID.BOX_SIZE >= data.Column &&
                        i.TextBox.Text == answer);
                }
            }

            if (_noteModeEnabled)
            {
                if (data.TextBox.Text.Length == 1)
                {
                    return;
                }

                switch (e.Text)
                {
                    case "1":
                        data.Labels[0].Content = data.Labels[0].Content == null ? "1" : null;
                        break;
                    case "2":
                        data.Labels[1].Content = data.Labels[1].Content == null ? "2" : null;
                        break;
                    case "3":
                        data.Labels[2].Content = data.Labels[2].Content == null ? "3" : null;
                        break;
                    case "4":
                        data.Labels[3].Content = data.Labels[3].Content == null ? "4" : null;
                        break;
                    case "5":
                        data.Labels[4].Content = data.Labels[4].Content == null ? "5" : null;
                        break;
                    case "6":
                        data.Labels[5].Content = data.Labels[5].Content == null ? "6" : null;
                        break;
                    case "7":
                        data.Labels[6].Content = data.Labels[6].Content == null ? "7" : null;
                        break;
                    case "8":
                        data.Labels[7].Content = data.Labels[7].Content == null ? "8" : null;
                        break;
                    case "9":
                        data.Labels[8].Content = data.Labels[8].Content == null ? "9" : null;
                        break;
                    default:
                        break;
                }
            }
            else if (data.TextBox.Opacity == 0)
            {
                for (int i = 0; i < data.Labels.Count(); i++)
                {
                    data.Labels[i].Content = null;
                }

                data.TextBox.Opacity++;
            }

            if (!_noteModeEnabled && e.Text != answer)
            {
                data.TextBox.Foreground = Brushes.Red;
            }
            else if(!_noteModeEnabled)
            {
                data.TextBox.Foreground = Brushes.Black;
                data.Labels = null;
                Keyboard.ClearFocus();
            }

            e.Handled = true;

            data.TextBox.Text = _noteModeEnabled ? null : e.Text;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void NoteMode_Checked(object sender, RoutedEventArgs e)
        {
            _noteModeEnabled = true;

            foreach (var item in _cellsData)
            {
                var data = item.Value;

                // Если ячейка пуста или есть заметки, то скрываем поле для ввода
                if (
                     data.Labels != null && (
                     data.TextBox.Text.Length == 0 ||
                     data.Labels.FirstOrDefault(i => i.Content != null) != null)
                   )
                {

                    data.TextBox.Opacity = 0;
                }
            }
        }

        private void NoteMode_Unchecked(object sender, RoutedEventArgs e)
        {
            _noteModeEnabled = false;

            foreach (var item in _cellsData)
            {
                var data = item.Value;

                // Если подсказки не добавлены, то показываем поля ввода
                if (data.Labels != null && data.Labels.FirstOrDefault(i => i.Content != null) == null)
                {
                    data.TextBox.Opacity = 1;
                }
            }
        }

        private void ErrorPrevention_Checked(object sender, RoutedEventArgs e)
        {
            _errorPreventionModeEnabled = true;
        }

        private void ErrorPrevention_Unchecked(object sender, RoutedEventArgs e)
        {
            _errorPreventionModeEnabled = false;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _borders[((TextBox)sender).GetHashCode()].BorderBrush = 
                new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _borders[((TextBox)sender).GetHashCode()].BorderBrush = Brushes.Gray;
        }

        private void Menu_Easy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
