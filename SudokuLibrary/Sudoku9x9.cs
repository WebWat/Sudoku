﻿using SudokuLibrary.Base;

namespace SudokuLibrary
{
    public class Sudoku9x9 : Sudoku_x_
    {
        public const int Size = 9;
        public const int BoxSize = 3;

        private readonly Random _random = new Random();
        private readonly List<int> _values = new();
        private const int _iterations = 10;

        public Sudoku9x9(Difficult difficult, Algorithms algorithm = Algorithms.BruteForce)
            : base(difficult, algorithm, Size, BoxSize)
        {
            int square = Size * Size;
            bool failed;
            int temp = 0;
            int index = 0;

            for (int i = 0; i < square; i++)
            {
                _values.Add(i);
                index = _random.Next(i + 1);
                temp = _values[index];
                _values[index] = i;
                _values[i] = temp;
            }

            do
            {
                failed = false;

                Generate();

                switch (difficult)
                {
                    case Difficult.Dev:
                        failed = ZeroFillWithCheck(10);
                        break;
                    case Difficult.Easy:
                        failed = ZeroFillWithCheck(46);
                        break;
                    case Difficult.Medium:
                        failed = ZeroFillWithCheck(51);
                        break;
                    case Difficult.Hard:
                        failed = ZeroFillWithCheck(56);
                        break;
                }
            }
            while (failed);
        }

        private bool ZeroFillWithCheck(int max)
        {
            int current = 0;
            int i = 0;
            int x = 0;
            int y = 0;
            int temp = 0;
            int nextIndex = 0;
            int offset = 1;

            while (i < max)
            {
                current = _values[i];

                x = current / Size;
                y = current - x * Size;

                temp = Generated[x, y];
                Generated[x, y] = 0;

                if (!TrySolve())
                {
                    Generated[x, y] = temp;

                    temp = _values[i];
                    nextIndex = i + offset++;

                    _values[i] = _values[nextIndex];
                    _values[nextIndex] = temp;

                    if (nextIndex + 1 == _values.Count)
                    {
                        //Console.WriteLine("failed");
                        return true;
                    }
                }
                else
                {
                    i++;
                    offset = 1;
                }
            }


            return false;
        }

        protected override void Generate()
        {
            int[,] initial = new int[Size, Size]
            {
                { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                { 9, 1, 2, 3, 4, 5, 6, 7, 8 },
            };

            for (int i = 0; i < _iterations; i++)
            {
                switch (_random.Next(0, 5))
                {
                    case 0:
                        SwapRowsLine(initial);
                        break;
                    case 1:
                        SwapColumnsLine(initial);
                        break;
                    case 2:
                        SwapRowsAreas(initial);
                        break;
                    case 3:
                        SwapColumnsAreas(initial);
                        break;
                    default:
                        initial = Transpose(initial);
                        break;
                }
            }

            Generated = initial;
        }

        private void SwapRowsLine(int[,] matrix)
        {
            var initialSpot = _random.Next(0, 2);
            SwapRows(initialSpot, initialSpot + 1, matrix);

            initialSpot = _random.Next(3, 5);
            SwapRows(initialSpot, initialSpot + 1, matrix);

            initialSpot = _random.Next(6, 8);
            SwapRows(initialSpot, initialSpot + 1, matrix);
        }

        private void SwapColumnsLine(int[,] matrix)
        {
            var initialSpot = _random.Next(0, 2);
            SwapColumns(initialSpot, initialSpot + 1, matrix);

            initialSpot = _random.Next(3, 5);
            SwapColumns(initialSpot, initialSpot + 1, matrix);

            initialSpot = _random.Next(6, 8);
            SwapColumns(initialSpot, initialSpot + 1, matrix);
        }

        private void SwapRowsAreas(int[,] matrix)
        {
            var firstArea = _random.Next(0, 3);
            var secondArea = _random.Next(0, 3);

            if (firstArea == secondArea)
            {
                secondArea = secondArea < 2 ? secondArea + 1 : secondArea;
                firstArea = firstArea == secondArea ? firstArea - 1 : firstArea;
            }
            else
            {
                var temp = firstArea;
                firstArea = Math.Min(firstArea, secondArea);
                secondArea = Math.Max(temp, secondArea);
            }

            firstArea *= 3;
            secondArea *= 3;

            for (int j = 0; j < 3; j++)
            {
                SwapRows(firstArea + j, secondArea + j, matrix);
            }
        }

        private void SwapColumnsAreas(int[,] matrix)
        {
            var firstArea = _random.Next(0, 3);
            var secondArea = _random.Next(0, 3);

            if (firstArea == secondArea)
            {
                secondArea = secondArea < 2 ? secondArea + 1 : secondArea;
                firstArea = firstArea == secondArea ? firstArea - 1 : firstArea;
            }
            else
            {
                var temp = firstArea;
                firstArea = Math.Min(firstArea, secondArea);
                secondArea = Math.Max(temp, secondArea);
            }

            firstArea *= 3;
            secondArea *= 3;

            for (int j = 0; j < 3; j++)
            {
                SwapColumns(firstArea + j, secondArea + j, matrix);
            }
        }

        private int[,] Transpose(int[,] matrix)
        {
            var newArray = new int[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    newArray[j, i] = matrix[i, j];
                }
            }

            return newArray;
        }

        private void SwapColumns(int firstIndex, int secondIndex, int[,] matrix)
        {
            int temp = 0;

            for (int i = 0; i < Size; i++)
            {
                temp = matrix[i, firstIndex];
                matrix[i, firstIndex] = matrix[i, secondIndex];
                matrix[i, secondIndex] = temp;
            }
        }

        private void SwapRows(int firstIndex, int secondIndex, int[,] matrix)
        {
            int temp = 0;

            for (int i = 0; i < Size; i++)
            {
                temp = matrix[firstIndex, i];
                matrix[firstIndex, i] = matrix[secondIndex, i];
                matrix[secondIndex, i] = temp;
            }
        }
    }
}