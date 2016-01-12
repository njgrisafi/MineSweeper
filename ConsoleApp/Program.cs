using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineSweeper;

namespace ConsoleApp
{
    class Program
    {
        static void PrintBoard(Cell[,] boardCells, int height, int width)
        {
            for (var r = 0; r < height; r++)
            {
                for (var c = 0; c < width; c++)
                {
                    var cell = boardCells[r, c];
                    if (cell.IsExposed)
                    {
                        if (cell.IsBomb && cell.IsFlipped)
                        {
                            Console.Write("B");
                        } else if (cell.IsFlipped)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            Console.Write(cell.Number);
                        }
                    }
                    else
                    {
                        Console.Write("?");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Console version of mine sweeper
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var height = 4;
            var width = 4;
            var numbumbs = 4;
            var engine = new Engine(height, width, numbumbs);
            while (engine.GameState == GameState.Running)
            {
                var board = engine.GetBoardAs2DArray();
                PrintBoard(board, height, width);
                Console.WriteLine("Row: ");
                var row = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Col: ");
                var col = Convert.ToInt32(Console.ReadLine());
                var userMove = new UserMove(row, col, false);
                engine.MakeMove(userMove);
            }
            PrintBoard(engine.GetBoardAs2DArray(), height, width);
            Console.WriteLine(engine.GameState == GameState.Win ? "YOU WIN!!!" : "YOU LOSE!!!");
            Console.ReadKey();
        }
    }
}
