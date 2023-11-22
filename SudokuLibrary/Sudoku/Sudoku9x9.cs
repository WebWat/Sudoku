using SudokuLibrary.SolvingAlgorithms;

namespace SudokuLibrary.Sudoku
{
    public class Sudoku9x9 : Sudoku_x_
    {
        private readonly int[] _values;

        public Sudoku9x9(Difficult difficult, Algorithms algorithm = Algorithms.BruteForce)
            : base(difficult, algorithm, 9, 3)
        {
            int square = Size * Size;
            _values = new int[Size * Size];
            bool failed = false;
            int temp, index;

            for (int i = 0; i < square; i++)
            {
                _values[i] = i;
                index = _random.Next(i + 1);
                temp = _values[index];
                _values[index] = i;
                _values[i] = temp;
            }

            Generate();

            var clone = (int[,])Generated.Clone();

            do
            {
                if (failed)
                {
                    Shuffle(_values);
                    Generated = (int[,])clone.Clone();
                    failed = false;
                }

                switch (difficult)
                {
                    case Difficult.Dev:
                        failed = FillWithZeros(10);
                        break;
                    case Difficult.Easy:
                        failed = FillWithZeros(46);
                        break;
                    case Difficult.Medium:
                        failed = FillWithZeros(51);
                        break;
                    case Difficult.Hard:
                        failed = FillWithZeros(56);
                        break;
                }
            }
            while (failed);
        }

        private void Shuffle(int[] array)
        {
            int k, value, n = array.Length;
            while (n > 1)
            {
                n--;
                k = _random.Next(n + 1);
                value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        private bool FillWithZeros(int max)
        {
            int i = 0;
            int offset = 1;
            int current, x, y, temp, nextIndex;

            while (i < max)
            {
                current = _values[i];

                x = current / Size;
                y = current - x * Size;

                temp = Generated[x, y];
                Generated[x, y] = 0;

                if (i > 8 && !TrySolve())
                {
                    Generated[x, y] = temp;

                    temp = _values[i];
                    nextIndex = i + offset++;

                    _values[i] = _values[nextIndex];
                    _values[nextIndex] = temp;

                    if (nextIndex + 1 == _values.Length)
                        return true;
                }
                else
                {
                    i++;
                    offset = 1;
                }
            }

            return false;
        }
    }
}