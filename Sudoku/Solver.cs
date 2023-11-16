using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    internal class Solver
    {
        private readonly List<Cell> _cellsToSolve = new();
        private readonly Cell[,] _cells = new Cell[SUDOKU_GRID.SIZE, SUDOKU_GRID.SIZE];

        public Cell this[int i, int j]
        {
            get { return _cells[i, j]; }
        }

        public Solver(string[] input)
        {
            for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.SIZE; j++)
                {
                    _cells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Solved = input[i][j] != '0',
                        Number = input[i][j] - '0'
                    };

                    if (!_cells[i, j].Solved)
                        _cellsToSolve.Add(_cells[i, j]);
                }
            }
        }

        public bool TrySolve()
        {
            return SolveNext(0);
        }

        private bool SolveNext(int index)
        {
            if (index == _cellsToSolve.Count)
                return true;

            var cell = _cellsToSolve[index];
            var markers = GetMarkers(cell);

            cell.Solved = true;

            foreach (var marker in markers)
            {
                cell.Number = marker;

                if (SolveNext(index + 1))
                    return true;
            }

            cell.Solved = false;
            return false;
        }

        private List<int> GetMarkers(Cell cell)
        {
            var row = GetRow(cell.X);

            var column = GetColumn(cell.Y);

            var box = GetBox(cell.X, cell.Y);

            var solvedNumbers = row.Concat(column)
                                   .Concat(box)
                                   .Where(x => x.Solved)
                                   .Select(x => x.Number)
                                   .ToHashSet();

            var markers = new List<int>();

            for (int possibleNumber = 1; possibleNumber <= SUDOKU_GRID.SIZE; possibleNumber++)
            {
                if (!solvedNumbers.Contains(possibleNumber))
                    markers.Add(possibleNumber);
            }

            return markers;
        }

        private IEnumerable<Cell> GetRow(int index)
        {
            for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
                yield return _cells[index, i];
        }

        private IEnumerable<Cell> GetColumn(int index)
        {
            for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
                yield return _cells[i, index];
        }

        private IEnumerable<Cell> GetBox(int iIndex, int jIndex)
        {
            var iBox = iIndex / SUDOKU_GRID.BOX_SIZE;
            var jBox = jIndex / SUDOKU_GRID.BOX_SIZE;

            for (int i = iBox * SUDOKU_GRID.BOX_SIZE; i < (iBox + 1) * SUDOKU_GRID.BOX_SIZE; i++)
                for (int j = jBox * SUDOKU_GRID.BOX_SIZE; j < (jBox + 1) * SUDOKU_GRID.BOX_SIZE; j++)
                    yield return _cells[i, j];
        }

        public static string[] Generate(Difficult difficult)
        {
            return new string[]
            {
                "378410200",
                "560008000",
                "000760001",
                "000300800",
                "032100690",
                "006284357",
                "004000005",
                "050031946",
                "610000708",
            };
        }
    }
}
