using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class UserMove
    {
        public bool IsFlag { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }

        public UserMove(int row, int col, bool isFlag)
        {
            Row = row;
            Col = col;
            IsFlag = isFlag;
        }

    }
}
