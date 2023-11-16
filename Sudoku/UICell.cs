using System.Windows.Controls;

namespace Sudoku
{
    internal class UICell
    {
        public int Row { get; init; }
        public int Column { get; init; }
        public TextBox TextBox { get; init; }
        public Label[] Labels { get; set; }
    }
}
