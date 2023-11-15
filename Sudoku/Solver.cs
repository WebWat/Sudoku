using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    internal class Solver
    {
        private readonly List<Cell> _cellsToSolve = new();
        private string[] _sudoku = new[] { "IMPOSSIBLE" };
        private readonly Cell[,] _cells = new Cell[SUDOKU_GRID.SIZE, SUDOKU_GRID.SIZE];

        public Solver()
        {
            for (int i = 0; i < SUDOKU_GRID.SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_GRID.SIZE; j++)
                {
                    _cells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Solved = _sudoku[i][j] != 'x',
                        Number = _sudoku[i][j] - '0'
                    };

                    if (!_cells[i, j].Solved)
                        _cellsToSolve.Add(_cells[i, j]);
                }
            }
        }

        public void Input()
        {
            _sudoku = new string[SUDOKU_GRID.SIZE];

            _sudoku[0] = "1x6xxxx3x";
            _sudoku[1] = "x2xx184xx";
            _sudoku[2] = "xxx7xxxxx";
            _sudoku[3] = "3xxx75x4x";
            _sudoku[4] = "xxx2xx7xx";
            _sudoku[5] = "x5x9xxxxx";
            _sudoku[6] = "xxxxx9xxx";
            _sudoku[7] = "x8xx541xx";
            _sudoku[8] = "2xxxxxxx8";
        }

        public string[] Solve()
        {
            return SolveNext(0)
                 ? new[] { "Solved" }
                 : new[] { "IMPOSSIBLE" };
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
    }
}
