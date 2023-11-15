using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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
        private Dictionary<int, (TextBox, Label[])> _cellsData = new Dictionary<int, (TextBox, Label[])>();
        private Dictionary<int, Border> _borders = new Dictionary<int, Border>();

        private const int _rowOffset = 2;
        private const int _columnOffset = 1;

        private BrushConverter _converter = new();
        private bool _noteModeEnabled = false;

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < SUDOKU_GRID.BOX_SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.BOX_SIZE; j++)
                {
                    CreateBox(i + _rowOffset, j + _columnOffset);
                }
            }
        }

        private void CreateBox(int row, int column)
        {
            var boxBorder = new Border();

            boxBorder.BorderBrush = _converter.ConvertFromString("Black") as Brush;
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

            //container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5) });
            //container.RowDefinitions.Add(new RowDefinition());
            //container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5) });

            //container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5) });
            //container.ColumnDefinitions.Add(new ColumnDefinition());
            //container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5) });

            var border = new Border();
            //border.SetValue(Grid.RowProperty, 1);
            //border.SetValue(Grid.ColumnProperty, 1);

            var labelsGrid = new UniformGrid();
            var textBox = new TextBox();
            //textBox.SetValue(Grid.RowProperty, 1);
            //textBox.SetValue(Grid.ColumnProperty, 1);

            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            textBox.GotFocus += UniformGrid_GotFocus;
            textBox.LostFocus += UniformGrid_LostFocus;

            var _labels = new Label[SUDOKU_GRID.SIZE];

            for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
            {
                var label = new Label();

                _labels[i] = label;
                labelsGrid.Children.Add(label);
            }

            border.Child = labelsGrid;

            container.Children.Add(border);
            _borders.Add(textBox.GetHashCode(), border);
            _cellsData.Add(textBox.GetHashCode(), (textBox, _labels));
            container.Children.Add(textBox);

            return container;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            (TextBox textBox, Label[] labels) = _cellsData[((TextBox)sender).GetHashCode()];

            if (_noteModeEnabled)
            {
                switch (e.Text)
                {
                    case "1":
                        labels[0].Content = labels[0].Content == null ? "1" : null;
                        break;
                    case "2":
                        labels[1].Content = labels[1].Content == null ? "2" : null;
                        break;
                    case "3":
                        labels[2].Content = labels[2].Content == null ? "3" : null;
                        break;
                    case "4":
                        labels[3].Content = labels[3].Content == null ? "4" : null;
                        break;
                    case "5":
                        labels[4].Content = labels[4].Content == null ? "5" : null;
                        break;
                    case "6":
                        labels[5].Content = labels[5].Content == null ? "6" : null;
                        break;
                    case "7":
                        labels[6].Content = labels[6].Content == null ? "7" : null;
                        break;
                    case "8":
                        labels[7].Content = labels[7].Content == null ? "8" : null;
                        break;
                    case "9":
                        labels[8].Content = labels[8].Content == null ? "9" : null;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (textBox.Opacity++ == 0)
                {
                    for (int i = 0; i < labels.Count(); i++)
                    {
                        labels[i].Content = null;
                    }
                }
            }

            if (textBox.Text.Length == 1 || !int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            _noteModeEnabled = true;

            foreach (var item in _cellsData)
            {
                var (textBox, labels) = item.Value;

                // Если ячейка пуста или есть заметки, то скрываем поле для ввода
                if (textBox.Text.Length == 0 ||
                    labels.FirstOrDefault(i => i.Content != null) != null)
                {

                    textBox.Opacity = 0;
                }
                else
                {
                    textBox.IsEnabled = false;
                }
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _noteModeEnabled = false;

            foreach (var item in _cellsData)
            {
                var (textBox, labels) = item.Value;

                // Если ячейка пуста или есть заметки, то скрываем поле для ввода
                if (textBox.Text.Length != 0 && !textBox.IsEnabled)
                {

                    textBox.IsEnabled = true;
                }
                // Если подсказки не добавлены, то показываем поля ввода
                else if (labels.FirstOrDefault(i => i.Content != null) == null)
                {
                    textBox.Text = string.Empty;
                    textBox.Opacity = 1;
                }
            }
        }

        private void UniformGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            _borders[((TextBox)sender).GetHashCode()].BorderBrush = _converter.ConvertFromString("#569de5") as Brush;
        }

        private void UniformGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            _borders[((TextBox)sender).GetHashCode()].BorderBrush = _converter.ConvertFromString("Gray") as Brush;
        }

        private void Menu_Easy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
