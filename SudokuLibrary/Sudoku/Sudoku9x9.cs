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

        private protected override void Generate()
        {
            for (int i = 0; i < Size; i += BoxSize)
            {
                FillBox(i, i);
            }

            FillRemaining(0, BoxSize);
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
        private bool FillRemaining(int i, int j)
        {
            // Если дошли до конца строки, то переходим на новую
            if (j >= Size && i < Size - 1)
            {
                i++;
                j = 0;
            }

            // Если дошли до конца строки, то завершаем
            if (i >= Size && j >= Size)
                return true;

            // Пропускаем 1 диагональный квадрат
            if (i < BoxSize)
            {
                j = j < BoxSize ? BoxSize : j;
            }
            // Пропускаем 2 диагональный квадрат
            else if (i < Size - BoxSize)
            {
                j = j == i / BoxSize * BoxSize ? j + BoxSize : j;
            }
            // Пропускаем 3 диагональный квадрат
            else if (j == Size - BoxSize)
            {
                j = 0;
                if (++i >= Size)
                    return true;
            }

            for (int num = 1; num <= Size; num++)
            {
                if (CheckIfSafe(i, j, num))
                {
                    Generated[i, j] = num;

                    if (FillRemaining(i, j + 1))
                        return true;

                    Generated[i, j] = 0;
                }
            }

            return false;
        }

        private void FillBox(int row, int column)
        {
            int number;
            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    do
                    {
                        number = _random.Next(1, Size + 1);
                    }
                    while (!UsedInBox(row, column, number));

                    Generated[row + i, column + j] = number;
                }
            }
        }

        private bool CheckIfSafe(int i, int j, int number)
        {
            return UsedInRow(i, number) &&
                   UsedInCol(j, number) &&
                   UsedInBox(i - i % BoxSize, j - j % BoxSize, number);
        }

        bool UsedInBox(int rowStart, int colStart, int num)
        {
            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    if (Generated[rowStart + i, colStart + j] == num)
                        return false;
                }
            }

            return true;
        }

        private bool UsedInRow(int i, int number)
        {
            for (int j = 0; j < Size; j++)
            {
                if (Generated[i, j] == number)
                    return false;
            }

            return true;
        }

        private bool UsedInCol(int j, int number)
        {
            for (int i = 0; i < Size; i++)
            {
                if (Generated[i, j] == number)
                    return false;
            }

            return true;
        }

    }
}