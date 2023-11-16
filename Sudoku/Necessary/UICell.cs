using System.Windows.Controls;

namespace Sudoku.Necessary
{
    internal class UICell
    {
        public int Row { get; init; }
        public int Column { get; init; }
        public TextBox TextBox { get; init; } = default!;
        public Label[]? Labels { get; set; }
    }
}
