using SudokuLibrary.Base;

namespace SudokuLibrary
{
    public class BruteForce : Algorithm
    {
        private readonly Cell[,] _cells;
        private readonly List<Cell> _cellsToSolve = new();

        private int _iteration = 0;
        private readonly int _maxIterations;
        private readonly Dictionary<int, int> _solve = new();

        public BruteForce(int size, int boxSize, int maxIterations = 15_000) : base(size, boxSize)
        {
            _maxIterations = maxIterations;
            _cells = new Cell[size, size];
        }

        public override bool TrySolve(int[,] sudoku, int[,] result)
        {
            _iteration = 0;
            _solve.Clear();
            Initialize(sudoku);
            var isSolved = SolveNextFirst(0);

            if (isSolved)
            {
                Initialize(sudoku);
                isSolved = !SolveNextSecond(0, false);
            }

            if (isSolved)
            {
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        result[i, j] = _cells[i, j].Number;
                    }
                }
            }

            return isSolved;
        }

        private void Initialize(int[,] input)
        {
            _cellsToSolve.Clear();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _cells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Solved = input[i, j] != 0,
                        Number = input[i, j]
                    };

                    if (!_cells[i, j].Solved)
                    {
                        _cellsToSolve.Add(_cells[i, j]);
                    }
                }
            }
        }

        private bool SolveNextFirst(int index)
        {
            if (_iteration++ > _maxIterations)
                return false;

            if (index == _cellsToSolve.Count)
                return true;

            var cell = _cellsToSolve[index];

            var markers = GetMarkers(cell);

            cell.Solved = true;

            for (int i = 0; i < markers.Count; i++)
            {
                if (_iteration > _maxIterations)
                    return false;

                cell.Number = markers[i];

                if (SolveNextFirst(index + 1))
                {
                    _solve.Add(index, cell.Number);
                    return true;
                }
            }

            cell.Solved = false;
            return false;
        }

        private bool SolveNextSecond(int index, bool newPath)
        {
            if (index == _cellsToSolve.Count)
                return newPath;

            var cell = _cellsToSolve[index];
            var markers = GetMarkers(cell);
            cell.Solved = true;

            for (int i = 0; i < markers.Count; i++)
            {
                if (!newPath && markers[i] == _solve[index])
                {
                    continue;
                }
                else
                {
                    cell.Number = markers[i];
                }
                if (SolveNextSecond(index + 1, true))
                {
                    return true;
                }
            }

            if (!newPath)
            {
                cell.Number = _solve[index];
                return SolveNextSecond(index + 1, false);
            }

            cell.Solved = false;
            return false;
        }

        private List<int> GetMarkers(Cell cell)
        {
            List<int> markers;

            markers = new();

            var solvedNumbers = GetAll(cell.X, cell.Y);

            if (solvedNumbers.Count == 9)
            {
                return markers;
            }

            for (int possibleNumber = 1; possibleNumber <= _size; possibleNumber++)
            {
                if (!solvedNumbers.Contains(possibleNumber))
                    markers.Add(possibleNumber);
            }

            return markers;
        }

        private List<int> GetAll(int x, int y)
        {
            var result = new List<int>();

            Cell cell;

            for (int i = 0; i < _size; i++)
            {
                cell = _cells[x, i];
                if (cell.Solved && !result.Contains(cell.Number)) result.Add(cell.Number);

                cell = _cells[i, y];
                if (cell.Solved && !result.Contains(cell.Number)) result.Add(cell.Number);
            }

            var iBox = x / _boxSize;
            var jBox = y / _boxSize;
            var iBoxMax = (iBox + 1) * _boxSize;
            var jBoxMax = (jBox + 1) * _boxSize;

            for (int i = iBox * _boxSize; i < iBoxMax; i++)
            {
                for (int j = jBox * _boxSize; j < jBoxMax; j++)
                {
                    cell = _cells[i, j];
                    if (cell.Solved && !result.Contains(cell.Number)) result.Add(cell.Number);
                }
            }

            return result;
        }
    }
}
