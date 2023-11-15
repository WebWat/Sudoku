﻿namespace Sudoku
{
    internal static class Generator
    {
        public static string[,] Generate()
        {
            return new string[,]
            {
                { "1", "0", "6", "0", "0", "0", "0", "3", "0" },
                { "0", "2", "0", "0", "1", "8", "4", "0", "0" },
                { "0", "0", "0", "7", "0", "0", "0", "0", "0" },
                { "3", "0", "0", "0", "7", "5", "0", "4", "0" },
                { "0", "0", "0", "2", "0", "0", "7", "0", "0" },
                { "0", "5", "0", "9", "0", "0", "0", "0", "0" },
                { "0", "0", "0", "0", "0", "9", "0", "0", "0" },
                { "0", "8", "0", "0", "5", "4", "1", "0", "0" },
                { "2", "0", "0", "0", "0", "0", "0", "0", "8" },
            };
        }
    }
}