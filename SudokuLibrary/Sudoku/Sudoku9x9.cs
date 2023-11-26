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
            // Создаем массив индексов
            _values = new int[square];
            bool failed = false;

            for (int i = 0; i < square; i++)
            {
                _values[i] = i;
            }

            Shuffle(_values);

            Generate();

            // Сохраняем копию сгенерированного массива,
            // т.к. Generated может измениться в процессе генерации
            var clone = (int[,])Generated.Clone();

            do
            {
                // Если не получись сгенерировать,
                // перемешиваем массив
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
            // Случайно заполняем диагональные квадраты
            for (int i = 0; i < Size; i += BoxSize)
            {
                FillBox(i, i);
            }

            FillRemaining(0, BoxSize);
        }

        // Метод перемешивания массива
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

        // Заполняем массив нулями
        private bool FillWithZeros(int max)
        {
            int i = 0;
            // Смещение элемента, относительно i
            int offset = 1;
            int current, x, y, temp, nextIndex;

            while (i < max)
            {
                current = _values[i];

                // Получаем индексы для двумерного массива через одномерный
                x = current / Size;
                y = current - x * Size;

                temp = Generated[x, y];
                Generated[x, y] = 0;

                // "Пропускаем первые 8 чисел" И если уникальное решение не найдено,
                // то меняем местами значение
                if (i > 8 && !TrySolve())
                {
                    Generated[x, y] = temp;

                    temp = _values[i];
                    // Увеличиваем индекс для подбора следующего элемента
                    // в случае неудачи
                    nextIndex = i + offset++;

                    // Берем следующий элемент
                    _values[i] = _values[nextIndex];
                    _values[nextIndex] = temp;

                    // Если достигли конца и ничего не нашли, выходим из метода 
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

        // Заполнение оставшихся ячеек
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

        // Заполнение квадрата случайными числами
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

        // Полная проверка
        private bool CheckIfSafe(int i, int j, int number)
        {
            return UsedInRow(i, number) &&
                   UsedInColumn(j, number) &&
                   UsedInBox(i - i % BoxSize, j - j % BoxSize, number);
        }

        // Проверка наличия похожего элемента в квадрате
        bool UsedInBox(int rowStart, int columnStart, int number)
        {
            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    if (Generated[rowStart + i, columnStart + j] == number)
                        return false;
                }
            }

            return true;
        }

        // Проверка наличия похожего элемента в строке
        private bool UsedInRow(int i, int number)
        {
            for (int j = 0; j < Size; j++)
            {
                if (Generated[i, j] == number)
                    return false;
            }

            return true;
        }

        // Проверка наличия похожего элемента в колонке
        private bool UsedInColumn(int j, int number)
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