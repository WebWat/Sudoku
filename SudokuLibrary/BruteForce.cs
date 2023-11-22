using SudokuLibrary.Base;

namespace SudokuLibrary
{
    public class BruteForce : Algorithm
    {
        private readonly Cell[,] _cells;
        private readonly int[,] _solution;
        private readonly List<Cell> _cellsToSolve = new();
        private int _solutionCount = 0;
        private int _iteration = 0;
        private readonly int _maxIterations;

        public BruteForce(int size, int boxSize, int maxIterations = 15_000) : base(size, boxSize)
        {
            _maxIterations = maxIterations;
            _cells = new Cell[size, size];
            _solution = new int[size, size];
        }

        public override bool TrySolve(int[,] sudoku, out int[,] result)
        {
            result = new int[_size, _size];
            _solutionCount = 0;
            _iteration = 0;
            _cellsToSolve.Clear();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _cells[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        Solved = sudoku[i, j] != 0,
                        Number = sudoku[i, j]
                    };

                    if (!_cells[i, j].Solved)
                    {
                        _cellsToSolve.Add(_cells[i, j]);
                    }
                }
            }

            SolveNext(0);

            if (_iteration > _maxIterations)
                return false;

            var isSolved = _solutionCount == 1;

            if (isSolved)
            {
                result = _solution;
            }

            return isSolved;
        }

        private bool SolveNext(int index)
        {
            _iteration++;

            if (index == _cellsToSolve.Count)
            {
                if (++_solutionCount == 1)
                {
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            _solution[i, j] = _cells[i, j].Number;
                        }
                    }
                }

                return true;
            }

            var cell = _cellsToSolve[index];

            var markers = GetMarkers(cell);

            cell.Solved = true;

            for (int i = 0; i < markers.Count; i++)
            {
                if (_iteration > _maxIterations)
                {
                    //Console.WriteLine("iteration f");
                    return false;
                }

                cell.Number = markers[i];

                if (SolveNext(index + 1))
                {
                    if (_solutionCount > 1) 
                        return true;
                }
            }

            cell.Solved = false;

            return false;
        }

        private List<int> GetMarkers(Cell cell)
        {
            List<int> markers;

            markers = new();

            var solvedNumbers = GetAll(cell.X, cell.Y);

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
                if (cell.Solved) result.Add(cell.Number);

                cell = _cells[i, y];
                if (cell.Solved) result.Add(cell.Number);
            }

            var iBox = x / _boxSize;
            var jBox = y / _boxSize;
            var iBoxMax = (iBox + 1) * _boxSize;
            var jBoxMax = (jBox + 1) * _boxSize;
            iBox *= _boxSize;
            jBox *= _boxSize;

            for (int i = iBox; i < iBoxMax; i++)
            {
                for (int j = jBox; j < jBoxMax; j++)
                {
                    cell = _cells[i, j];
                    if (cell.Solved) result.Add(cell.Number);
                }
            }

            return result;
        }
    }
}
