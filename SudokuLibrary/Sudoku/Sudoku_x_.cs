using SudokuLibrary.SolvingAlgorithms;

namespace SudokuLibrary.Sudoku
{
    public abstract class Sudoku_x_
    {
        public int[,] Generated = default!;
        public int[,] Solved = default!;
        public readonly Difficult Difficult;

        private protected readonly int Size = 0;
        private protected readonly int BoxSize = 0;
        private protected readonly Random _random = new();
        private protected readonly Algorithm _algorithm = default!;

        public Sudoku_x_(Difficult difficult, Algorithms algorithm, int size, int boxSize)
        {
            Size = size;
            BoxSize = boxSize;

            Generated = new int[Size, Size];

            Difficult = difficult;

            switch (algorithm)
            {
                case Algorithms.BruteForce:
                    _algorithm = new BruteForce(Size, BoxSize);
                    break;
                default:
                    break;
            }
        }

        private protected bool TrySolve()
            => _algorithm.TrySolve(Generated, out Solved);

        private protected abstract void Generate();
    }
}
