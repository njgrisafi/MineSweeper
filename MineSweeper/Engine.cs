using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Engine
    {
        public GameState GameState;

        private readonly int _numBombs;
        private readonly int _numCells;
        private readonly Board _gameBoard;
        private int _numFlips;
        private int _correctFlags;
        private int _numFlags;

        public Engine(int height, int width, int numBombs)
        {
            _numBombs = numBombs;
            _numCells = height * width;
            _correctFlags = 0;
            _numFlags = 0;
            _gameBoard = new Board(height, width);
            PlaceBombs();
            GameState = GameState.NotStarted;
        }
        #region public methods
        public void MakeMove(UserMove move)
        {
            var cell = _gameBoard.GetCell(move.Row, move.Col);
            if (cell == null || cell.IsFlipped)
            {
                return;
            }
            if (move.IsFlag)
            {
                if (!cell.IsFlagged)
                {
                    if (cell.IsBomb)
                    {
                        _correctFlags++;
                    }
                    _numFlags++;
                    cell.IsFlagged = true;
                }
                else
                {
                    if (cell.IsBomb)
                    {
                        _correctFlags--;
                    }
                    _numFlags--;
                    cell.IsFlagged = false;
                }
            }
            else
            {
                if (cell.IsFlagged)
                {
                    _numFlags--;
                    cell.IsFlagged = false;
                }
                if (cell.IsBomb)
                {
                    GameState = GameState.Loss;
                    cell.IsFlipped = true;
                    return;
                }
                if (cell.Number == 0)
                {
                    RevealEmptyCells(cell);
                }
                else
                {
                    cell.IsFlipped = true;
                    _numFlips += 1;
                }
            }
            if (!IsWin()) return;
            GameState = GameState.Win;
            RevealAllCells();
        }
        /// <summary>
        /// Get the height of the current board
        /// </summary>
        /// <returns>height of current board</returns>
        public int GetHeight()
        {
            return _gameBoard.Height;
        }
        /// <summary>
        /// Get the wifth of the current board
        /// </summary>
        /// <returns>width of current board</returns>
        public int GetWidth()
        {
            return _gameBoard.Width;
        }
        /// <summary>
        /// Convert the 1d board to 2d board object
        /// </summary>
        /// <returns>Cell[,] representation of the current board</returns>
        public Cell[,] GetBoardAs2DArray()
        {
            var board = new Cell[_gameBoard.Width, _gameBoard.Height];
            for (var r = 0; r < _gameBoard.Height; r++)
            {
                for (var c = 0; c < _gameBoard.Width; c++)
                {
                    board[c, r] = _gameBoard.GetCell(r, c);
                }
            }
            return board;
        }
        /// <summary>
        /// Get the current bombs remaining. Accounts for the flags place and total bombs on the board
        /// </summary>
        /// <returns>number of bombs remaining</returns>
        public int GetBombsRemaining()
        {
            return _numBombs - _numFlags;
        }
#endregion
        #region private methods
        /// <summary>
        /// Check if game is in win state
        /// </summary>
        /// <returns>true if win, o.w. false</returns>
        private bool IsWin()
        {
            if (_numFlips == _numCells - _numBombs)
            {
                return true;
            }
            return _correctFlags - _numBombs == 0;
        }
        /// <summary>
        /// Randomly place bombs on current board
        /// </summary>
        private void PlaceBombs()
        {
            for (var i = 0; i < _numBombs; i++)
            {
                var rand = new Random();
                var randomRow = rand.Next(0, _gameBoard.Height);
                var randomCol = rand.Next(0, _gameBoard.Width);
                var cell = _gameBoard.GetCell(randomRow, randomCol);
                while (cell.IsBomb)
                {
                    randomRow = rand.Next(0, _gameBoard.Height);
                    randomCol = rand.Next(0, _gameBoard.Width);
                    cell = _gameBoard.GetCell(randomRow, randomCol);
                }
                cell.IsBomb = true;
                cell.Number += 1;
                UpdateAdjacentCells(randomRow, randomCol);
            }
        }
        /// <summary>
        /// Get the adjacent cells from a given cell
        /// </summary>
        /// <param name="r">cell row</param>
        /// <param name="c">cell column</param>
        /// <returns>List of adjacent cell objects</returns>
        private IEnumerable<Cell> GetAdjacentCells(int r, int c)
        {
            var adjCells = new List<Cell>
            {
                _gameBoard.GetCell(r + 1, c),
                _gameBoard.GetCell(r - 1, c),
                _gameBoard.GetCell(r, c + 1),
                _gameBoard.GetCell(r, c - 1),
                _gameBoard.GetCell(r + 1, c + 1),
                _gameBoard.GetCell(r - 1, c - 1),
                _gameBoard.GetCell(r + 1, c - 1),
                _gameBoard.GetCell(r - 1, c + 1)
            };
            return adjCells.Where(cell => cell != null).ToList();
        }
        /// <summary>
        /// Update adjacent cells bomb count by 1
        /// </summary>
        /// <param name="r">row of current bomb cell</param>
        /// <param name="c">column of current bomb cell</param>
        private void UpdateAdjacentCells(int r, int c)
        {
            var adjCells = GetAdjacentCells(r, c);
            foreach (var adjCell in adjCells)
            {
                adjCell.Number += 1;
            }
        }
        /// <summary>
        /// BFS algorithm to reveal all empty cells from a given cell. Exposes "edge" cells as well.
        /// </summary>
        /// <param name="cell">starting cell</param>
        private void RevealEmptyCells(Cell cell)
        {
            var queue = new Queue<Cell>();
            queue.Enqueue(cell);
            while (queue.Count > 0)
            {
                var currCell = queue.Dequeue();
                currCell.IsFlipped = true;
                _numFlips += 1;
                if (currCell.Number != 0) continue;
                var adjCells = GetAdjacentCells(currCell.Row, currCell.Col);
                foreach (var adjCell in adjCells.Where(adjCell => !adjCell.IsBomb && !adjCell.IsFlipped))
                {
                    queue.Enqueue(adjCell);
                }
            }
        }
        /// <summary>
        /// Reveals all cells on the board. Sets the following cell properties:
        /// IsExposed = true
        /// IsFlipped = true
        /// IsFlagged = false
        /// </summary>
        private void RevealAllCells()
        {
            foreach (var cell in _gameBoard.Cells.Where(cell => !cell.IsFlipped))
            {
                cell.IsFlipped = true;
                cell.IsFlagged = false;
            }
        }
        #endregion
    }
}
