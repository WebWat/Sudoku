using SudokuLibrary.Base;
using System.Data.Common;
using System.Drawing;

namespace SudokuLibrary
{
    public class BruteForce : Algorithm
    {
        public override bool UniqueSolution => false;

        private readonly Cell[,] _cells;
        private readonly List<Cell> _cellsToSolve = new();

        private int _iteration = 0;
        private const int _maxIterations = 1500_000;
        private int _answers = -1;
        private Dictionary<int, int> dict = new();
        private int _maxDeep;

        public BruteForce(int size, int boxSize) : base(size, boxSize)
        {
            _cells = new Cell[size, size];
        }
        
        public override bool TrySolve(int[,] sudoku, int[,] result)
        {
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
                        _cellsToSolve.Add(_cells[i, j]);
                }
            }

            var isSolved = SolveNext(0, 0);


            if (isSolved)
            {
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
                            _cellsToSolve.Add(_cells[i, j]);
                    }
                }
                isSolved =  SolveNext(0, 0);
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

        private bool SolveNext(int index, int deep)
        {
            if (_iteration++ > _maxIterations)
                return false;

            if (index == _cellsToSolve.Count)
            {
                _answers = 1;
                return true;
            }

            var cell = _cellsToSolve[index];
            var markers = GetMarkers(cell);
            var value = dict.TryGetValue(deep, out _) ? 1 : -1;

            cell.Solved = true;

            for (int i = 0; i < markers.Count; i++)
            {
                if (_iteration > _maxIterations)
                    return false;

                if (value == 1)
                {
                    value--;
                    continue;
                }
                else
                    cell.Number = markers[i];

                if (SolveNext(index + 1, deep + 1))
                {
                    if (_answers != 1)
                        dict.Add(deep, cell.Number);
                    return true;
                }
            }

            if (value == 0)
            {
                dict.Remove(deep);
                return SolveNext(index, deep);
            }

            cell.Solved = false;
            return false;
        }

        private List<int> GetMarkers(Cell cell)
        {
            //var row = GetRow(cell.X);

            //var column = GetColumn(cell.Y);

            //var box = GetBox(cell.X, cell.Y);
            //var solvedNumbers = row.Concat(column)
            //                       .Concat(box)
            //                       .Where(x => x.Solved)
            //                       .Select(x => x.Number)
            //                       .ToList();

            var solvedNumbers = GetAll(cell.X, cell.Y);

            var markers = new List<int>();

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

            for (int i = iBox * _boxSize; i < iBoxMax; i++)
            {
                for (int j = jBox * _boxSize; j < jBoxMax; j++)
                {
                    cell = _cells[i, j];
                    if (cell.Solved) result.Add(cell.Number);
                }
            }

            return result;
        }

        private IEnumerable<Cell> GetRow(int index)
        {
            for (int i = 0; i < _size; i++)
                yield return _cells[index, i];
        }

        private IEnumerable<Cell> GetColumn(int index)
        {
            for (int i = 0; i < _size; i++)
                yield return _cells[i, index];
        }

        private IEnumerable<Cell> GetBox(int iIndex, int jIndex)
        {
            var iBox = iIndex / _boxSize;
            var jBox = jIndex / _boxSize;

            for (int i = iBox * _boxSize; i < (iBox + 1) * _boxSize; i++)
                for (int j = jBox * _boxSize; j < (jBox + 1) * _boxSize; j++)
                    yield return _cells[i, j];
        }
    }
}
