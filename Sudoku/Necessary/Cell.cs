namespace Sudoku.Necessary
{
    internal class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Solved { get; set; }
        public int Number { get; set; }

        public override string ToString()
            => Solved ? Number.ToString() : string.Empty;
    }
}
