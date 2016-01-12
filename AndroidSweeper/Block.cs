using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MineSweeper;

namespace AndroidSweeper
{
    public class Block : ImageButton
    {
        public bool IsExposed;
        public bool IsFlipped;
        public bool IsFlagged;
        public int Row;
        public int Col;

        public Block(Context context) : base(context) {
            Click += (sender, e) => {
                if (Game.GameState == GameState.NotStarted)
                {
                    Game.Start(); // start game if not running
                }

                if (!Game.IsOver)
                {
                    Game.MakeMove(Row, Col, false);
                }
            };
            LongClick += (sender, e) => {
                if (Game.GameState == GameState.NotStarted)
                {
                    Game.Start(); // start game if not running
                }
                if (Game.GameState == GameState.Running && !IsFlipped) // only set flag if not flipped
                {
                    Game.MakeMove(Row, Col, true);
                }
            };
            SetEmpty();
        }
        /// <summary>
        /// Set the block display as empty
        /// </summary>
        public void SetEmpty()
        {
            SetImageResource(Resource.Drawable.tile_empty);
            IsExposed = true;
            if (!IsFlagged) return;
            IsFlagged = false;
            Game.NumFlags++;
        }
        /// <summary>
        /// Set the block display as flip
        /// </summary>
        public void SetFlip()
        {
            SetImageResource(Resource.Drawable.tile_0);
            IsExposed = true;
            IsFlipped = true;
            if (!IsFlagged) return;
            IsFlagged = false;
            Game.NumFlags++;
        }
        /// <summary>
        /// Set the block display as flag
        /// </summary>
        public void SetFlag()
        {
            SetImageResource(Resource.Drawable.tile_flag);
            IsFlagged = true;
            Game.NumFlags--;
        }
        /// <summary>
        /// Set the block display as bomb
        /// </summary>
        public void SetBomb()
        {
            SetImageResource(Resource.Drawable.tile_bomb);
            IsExposed = true;
            IsFlipped = true;
        }
        /// <summary>
        /// Set the block display as a number
        /// </summary>
        /// <param name="number">number to be displayed</param>
        public void SetNumber(int number)
        {
            if (IsFlagged)
            {
                IsFlagged = false;
                Game.NumFlags++;
            }
            switch (number)
            {
                case 1:
                    SetImageResource(Resource.Drawable.tile_1);
                    break;
                case 2:
                    SetImageResource(Resource.Drawable.tile_2);
                    break;
                case 3:
                    SetImageResource(Resource.Drawable.tile_3);
                    break;
                case 4:
                    SetImageResource(Resource.Drawable.tile_4);
                    break;
                case 5:
                    SetImageResource(Resource.Drawable.tile_5);
                    break;
                case 6:
                    SetImageResource(Resource.Drawable.tile_6);
                    break;
                case 7:
                    SetImageResource(Resource.Drawable.tile_7);
                    break;
                case 8:
                    SetImageResource(Resource.Drawable.tile_8);
                    break;
                default:
                    SetEmpty();
                    break;
            }
            IsExposed = true;
        }
    }
}