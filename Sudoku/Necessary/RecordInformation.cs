using SudokuLibrary;
using System;

namespace Sudoku.Necessary
{
    internal class RecordInformation
    {
        public RecordInformation()
        {
        }

        public int Number { get; set; }
        public DateTime DateTime { get; set; }
        public Difficult Difficult { get; set; }
        public int Seconds { get; set; }
        public int Minutes { get; set; }
    }
}
