namespace SudokuLibrary.SolvingAlgorithms
{
    public abstract class Algorithm
    {
        private protected readonly int Size;
        private protected readonly int BoxSize;

        public Algorithm(int size, int boxSize)
        {
            Size = size;
            BoxSize = boxSize;
        }

        public abstract bool TrySolve(int[,] sudoku, out int[,] result);
    }
}
