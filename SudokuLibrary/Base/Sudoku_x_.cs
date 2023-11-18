namespace SudokuLibrary.Base
{
    public abstract class Sudoku_x_
    {
        public int[,] Generated;
        public int[,] Solved;

        protected readonly Algorithm _algorithm;

        public Sudoku_x_(Difficult difficult, Algorithms algorithm, int size, int boxSize)
        {
            Solved = new int[size, size];

            switch (algorithm)
            {
                case Algorithms.BruteForce:
                    _algorithm = new BruteForce(size, boxSize);
                    break;
                default:
                    break;
            }
        }

        protected abstract void Generate();

        protected bool TrySolve() => _algorithm.TrySolve(Generated, Solved);
    }
}
