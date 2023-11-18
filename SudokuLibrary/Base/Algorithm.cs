namespace SudokuLibrary.Base
{
    public abstract class Algorithm
    {
        protected readonly int _size;
        protected readonly int _boxSize;

        public abstract bool UniqueSolution { get; }

        public Algorithm(int size, int boxSize)
        {
            _size = size;
            _boxSize = boxSize;
        }

        public abstract bool TrySolve(int[,] sudoku, int[,] result);
    }
}
