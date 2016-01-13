using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Cell
    {

        public bool IsBomb { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsFlagged { get; set; }
        public int Number { get; set; }
        public int Row;
        public int Col;

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            IsFlipped = false;
            IsFlagged = false;
            IsBomb = false;
            Number = 0;
        }

    }
}
