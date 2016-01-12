using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Board
    {
        public List<Cell> Cells;
        public int Height;
        public int Width;

        public Board(int height, int width)
        {
            Height = height;
            Width = width;
            Cells = new List<Cell>();
            InitBoard();
        }
        #region public methods
        /// <summary>
        /// Get a cell from the board
        /// </summary>
        /// <param name="r">row of cell</param>
        /// <param name="c">column of cell</param>
        /// <returns></returns>
        public Cell GetCell(int r, int c)
        {
            if (r >= 0 && c >= 0 && r < Height && c < Width)
            {
                return Cells[Width * r + c];
            }
            return null;
        }
        /// <summary>
        /// Get a cell from the board
        /// </summary>
        /// <param name="cellIndex">index of current cell</param>
        /// <returns></returns>
        public Cell GetCell(int cellIndex)
        {
            return cellIndex < Height + Width ? Cells[cellIndex] : null;
        }
        /// <summary>
        /// Add a cell to the board
        /// </summary>
        /// <param name="cell">Cell to be added</param>
        public void AddCell(Cell cell)
        {
            Cells.Add(cell);
        }
        #endregion
        #region private methods
        /// <summary>
        /// Initializes a board of default cells
        /// </summary>
        private void InitBoard()
        {
            for (var r = 0; r < Height; r++)
            {
                for (var c = 0; c < Width; c++)
                {
                    Cells.Add(new Cell(r,c));
                }
            }
        }
        #endregion
    }
}
