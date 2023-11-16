using Sudoku.Necessary;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku
{
    public partial class MainWindow : Window
    {
        private Dictionary<int, UICell> _cellsData = new();
        // Словарь, необходимый для работы с UI
        private Dictionary<int, Border> _borders = new();

        // Смещение относительно BaseGrid
        private const int _rowOffset = 2;
        private const int _columnOffset = 1;

        // Решение судоку
        private Solver9x9 solver;

        // Конфликтные ячейки для выделения в режиме "Предотвращение ошибок"
        private TextBox? rowError;
        private TextBox? columnError;
        private TextBox? boxError;
        private TextBox? currentError;

        private readonly SolidColorBrush _focusBackgroundColor = new(Color.FromRgb(190, 230, 253));
        private readonly SolidColorBrush _errorForegroundColor = new(Color.FromRgb(192, 38, 38));

        public MainWindow()
        {
            InitializeComponent();

            solver = new Solver9x9(Solver9x9.Generate(Difficult.Easy));

            for (int i = 0; i < SUDOKU_GRID.BOX_SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.BOX_SIZE; j++)
                {
                    CreateBox(i + _rowOffset, j + _columnOffset);
                }
            }

            solver.TrySolve();
        }

        // Метод для создания UniformGrid размером 3x3
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

        // Метод для создания ячейки в UniformGrid
        private Grid CreateCell(int row, int column)
        {
            var container = new Grid();

            var indent = 0.05;
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(indent, GridUnitType.Star) });
            container.RowDefinitions.Add(new RowDefinition());
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(indent, GridUnitType.Star) });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(indent, GridUnitType.Star) });
            container.ColumnDefinitions.Add(new ColumnDefinition());
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(indent, GridUnitType.Star) });

            var border = new Border();
            border.SetValue(Grid.RowProperty, 0);
            border.SetValue(Grid.ColumnProperty, 0);
            border.SetValue(Grid.RowSpanProperty, 3);
            border.SetValue(Grid.ColumnSpanProperty, 3);

            var textBox = new TextBox();
            textBox.SetValue(Grid.RowProperty, 1);
            textBox.SetValue(Grid.ColumnProperty, 1);

            Label[]? labels = default;

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

        // Возвращаем значения по умолчанию для ячеек ошибок
        // и удаляем ссылки на них
        private void ClearErrorCells(bool isUnchecked)
        {
            if (rowError != null)
            {
                rowError.Foreground = Brushes.Black;
                rowError = null;
            }

            if (columnError != null)
            {
                columnError.Foreground = Brushes.Black;
                columnError = null;
            }

            if (boxError != null)
            {
                boxError.Foreground = Brushes.Black;
                boxError = null;
            }

            if (currentError != null)
            {
                if (isUnchecked)
                {
                    currentError.Background = Brushes.White;
                    _borders[currentError.GetHashCode()].Background = Brushes.White;
                }
                else
                {
                    currentError.Background = _focusBackgroundColor;
                    _borders[currentError.GetHashCode()].Background = _focusBackgroundColor;
                }

                currentError = null;
            }
        }

        // Обработчики событий
        // ********************************
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var data = _cellsData[((TextBox)sender).GetHashCode()];

            // Если значение сгенерировано или ввод не число, то пропускаем
            if (data.Labels == null || !int.TryParse(e.Text, out _))
            {
                return;
            }

            // Получаем правильно значение для текущего поля
            var answer = solver[data.Row, data.Column].ToString();

            // Отклоняем ввод
            e.Handled = true;

            // Если включен режим "Создание заметок"
            if (NoteMode.IsChecked == true)
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

                data.TextBox.Text = null;

                return;
            }

            // Если вышли из режима "Создание заметок", но остались заметки,
            // то очищаем заметки и показываем поле ввода
            if (data.TextBox.Opacity == 0)
            {
                for (int i = 0; i < data.Labels.Count(); i++)
                {
                    data.Labels[i].Content = null;
                }

                data.TextBox.Opacity++;
            }

            // Если включен режим "Предотвращение ошибок"
            if (ErrorPreventionMode.IsChecked == true)
            {
                // Очищаем конфликтные ячейки
                ClearErrorCells(false);

                // Если ответ неверен, то выделяем конфликтующие ячейки
                if (e.Text != answer)
                {
                    // Ищем ошибку в строке
                    rowError = _cellsData.Values.FirstOrDefault(cell =>
                        cell.Row == data.Row && cell.TextBox.Text == e.Text)?.TextBox;

                    // Ищем ошибку в столбце
                    columnError = _cellsData.Values.FirstOrDefault(cell =>
                        cell.Column == data.Column && cell.TextBox.Text == e.Text)?.TextBox;

                    var rowLeftBorder = data.Row - (data.Row % SUDOKU_GRID.BOX_SIZE);
                    var rowRightBorder = rowLeftBorder + SUDOKU_GRID.BOX_SIZE;
                    var columnLeftBorder = data.Column - (data.Column % SUDOKU_GRID.BOX_SIZE);
                    var columnRightBorder = columnLeftBorder + SUDOKU_GRID.BOX_SIZE;

                    // Ищем ошибку в квадрате
                    boxError = _cellsData.Values.FirstOrDefault(cell =>
                        cell.Row >= rowLeftBorder && cell.Row < rowRightBorder &&
                        cell.Column >= columnLeftBorder && cell.Column < columnRightBorder &&
                        cell.TextBox.Text == e.Text)?.TextBox;

                    // Если нашли, то меняем цвет
                    if (rowError != null) rowError.Foreground = _errorForegroundColor;
                    if (columnError != null) columnError.Foreground = _errorForegroundColor;
                    if (boxError != null) boxError.Foreground = _errorForegroundColor;

                    // Если ничего не нашли, то выделяем текущую ячейку
                    if (rowError == null && columnError == null && boxError == null)
                    {
                        data.TextBox.Background = _errorForegroundColor;
                        currentError = data.TextBox;
                        _borders[data.TextBox.GetHashCode()].Background = _errorForegroundColor;
                    }
                }
                else
                {
                    data.TextBox.Text = e.Text;
                    data.Labels = null;
                }

                return;
            }

            // Если ответ неверный, то выделяем цифру красным
            if (e.Text != answer)
            {
                data.TextBox.Foreground = _errorForegroundColor;
            }
            // Иначе фиксируем правильный ответ
            else
            {
                data.TextBox.Foreground = Brushes.Black;
                data.Labels = null;
                Keyboard.ClearFocus();
            }

            data.TextBox.Text = e.Text;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    e.Handled = true;
                    break;
                case Key.Back:
                case Key.Delete:
                    ((TextBox)sender).Text = null;
                    break;
                default:
                    break;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            // Меняем цвет при фокусе
            textbox.Background = _focusBackgroundColor;
            _borders[textbox.GetHashCode()].Background = _focusBackgroundColor;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            // Возвращаем цвет при потере фокуса
            textbox.Background = Brushes.White;
            _borders[textbox.GetHashCode()].Background = Brushes.White;
        }

        private void NoteMode_Checked(object sender, RoutedEventArgs e)
        {
            if (ErrorPreventionMode.IsChecked == true)
            {
                NoteMode.IsChecked = false;
                return;
            }

            foreach (var item in _cellsData)
            {
                var data = item.Value;

                // Если ячейка не задана по умолчанию И (пуста ИЛИ есть заметки), то скрываем поле для ввода
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

            foreach (var item in _cellsData)
            {
                var data = item.Value;

                // Если ячейка не задана по умолчанию И подсказки не добавлены,
                // то показываем поля ввода
                if (data.Labels != null && data.Labels.FirstOrDefault(i => i.Content != null) == null)
                {
                    data.TextBox.Opacity = 1;
                }
            }
        }

        private void ErrorPreventionMode_Checked(object sender, RoutedEventArgs e)
        {
            if (NoteMode.IsChecked == true)
            {
                ErrorPreventionMode.IsChecked = false;
                return;
            }

            foreach (var item in _cellsData)
            {
                var data = item.Value;

                // Если введены неверные ответы, то очищаем ячейки
                if (data.TextBox.Text != solver[data.Row, data.Column].ToString())
                {
                    data.TextBox.Text = null;
                    data.TextBox.Foreground = Brushes.Black;
                }
            }
        }

        private void ErrorPreventionMode_Unchecked(object sender, RoutedEventArgs e)
        {
            // Очищаем конфликтные ячейки
            ClearErrorCells(true);
        }

        private void Menu_Easy_Click(object sender, RoutedEventArgs e)
        {

        }
        // ********************************
    }
}
