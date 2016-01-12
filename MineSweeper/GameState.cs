using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Used to describe the current state of the game
    /// </summary>
    public enum GameState
    {
        Win,
        Loss,
        Running,
        NotStarted
    };
}