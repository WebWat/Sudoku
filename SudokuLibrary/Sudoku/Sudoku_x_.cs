﻿using SudokuLibrary.SolvingAlgorithms;

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

            Generated = new int[size, size];

            Difficult = difficult;

            switch (algorithm)
            {
                case Algorithms.BruteForce:
                    _algorithm = new BruteForce(size, boxSize);
                    break;
                default:
                    break;
            }
        }

        private protected bool TrySolve()
            => _algorithm.TrySolve(Generated, out Solved);

        private protected void Generate()
        {
            for (int i = 0; i < Size; i += BoxSize)
            {
                FillBox(i, i);
            }

            FillRemaining(0, BoxSize);
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