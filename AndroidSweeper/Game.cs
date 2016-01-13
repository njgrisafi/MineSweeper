using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MineSweeper;

namespace AndroidSweeper
{
    [Activity(Label = "Game", Theme = "@android:style/Theme.DeviceDefault.NoActionBar",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class Game : Activity
    {

        public static GameState GameState => _gameEngine.GameState; // Link to engine gamestate
        public static Chronometer Timer;
        public static TextView MinesRemainingTextView;
        public static bool IsOver;
        public static int NumFlags;

        private static Engine _gameEngine;
        private static Block[,] _blocks;
        private TableLayout _mineTable;
        private AlertDialog.Builder _lossAlert;
        private AlertDialog.Builder _winAlert;
        private Thread _runnerThread;
        private bool _run;

        #region public methods
        /// <summary>
        /// Make move and send to engine
        /// </summary>
        /// <param name="row">move row</param>
        /// <param name="col">move col</param>
        /// <param name="isFlag">true if move is a flag, o.w. flase</param>
        public static void MakeMove(int row, int col, bool isFlag)
        {
            if (isFlag && NumFlags != 0 || !isFlag)
            {
                var move = new UserMove(row, col, isFlag);
                _gameEngine.MakeMove(move);
                SetMinesText();
                UpdateTable();
            }
        }
        /// <summary>
        /// Start the game and timer
        /// </summary>
        public static void Start()
        {
            _gameEngine.GameState = GameState.Running;
            StartTimer();
        }
        /// <summary>
        /// Popup for a game loss
        /// </summary>
        public void LossPopup()
        {
            _lossAlert = new AlertDialog.Builder(this);
            _lossAlert.SetTitle("You loss!");
            _lossAlert.SetPositiveButton("Retry", (object sender, DialogClickEventArgs e) =>
            {
                this.Recreate();
            });

            _lossAlert.SetNegativeButton("Return to menu", (object sender, DialogClickEventArgs e) =>
            {
                Finish();
            });


            RunOnUiThread(() =>
            {
                _lossAlert.Show();
            });
        }
        /// <summary>
        /// Popup for a game win
        /// </summary>
        public void WinPopup()
        {
            _winAlert = new AlertDialog.Builder(this);
            _winAlert.SetTitle("You win!");
            _winAlert.SetPositiveButton("Play Again", (object sender, DialogClickEventArgs e) =>
            {
                this.Recreate();
            });

            _winAlert.SetNegativeButton("Return to menu", (object sender, DialogClickEventArgs e) =>
            {
                Finish();
            });

            RunOnUiThread(() =>
            {
                _winAlert.Show();
            });
        }
        #endregion
        #region protected methods
        /// <summary>
        /// Initial function call when activity loads
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            IsOver = false; // game not over
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Game);

            // Grab UI Componets
            _mineTable = FindViewById<TableLayout>(Resource.Id.mineTableLayout);
            MinesRemainingTextView = FindViewById<TextView>(Resource.Id.minesRemaining);
            Timer = FindViewById<Chronometer>(Resource.Id.chronometer);
            //var height = int.Parse((FindViewById<EditText>(Resource.Id.editTextHeight)).Text);
            //var width = int.Parse((FindViewById<EditText>(Resource.Id.editTextWidth)).Text);
            //var numMines = int.Parse((FindViewById<EditText>(Resource.Id.editTextMines)).Text);

            var numMines = int.Parse(Intent.GetStringExtra("mines") ?? "5");
            var height = int.Parse(Intent.GetStringExtra("height") ?? "11");
            var width = int.Parse(Intent.GetStringExtra("width") ?? "6");

            NumFlags = numMines;

            // Create new engine with current game settings
            _gameEngine = new Engine(height, width, numMines);
            SetMinesText();
            PopulateTable();
            _run = true; // Set runner thread to loop
            _runnerThread = new Thread(new ThreadStart(Runner));
            _runnerThread.Start(); // start runner
        }
        /// <summary>
        /// When activity is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            Console.WriteLine("Gameview got killed");
            base.OnDestroy();
        }
        #endregion
        #region private methods
        /// <summary>
        /// Update the UI table from engine data
        /// </summary>
        private static void UpdateTable()
        {
            var height = _gameEngine.GetHeight();
            var width = _gameEngine.GetWidth();
            var cells = _gameEngine.GetBoardAs2DArray();
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var cell = cells[col, row];
                    var block = _blocks[col, row];
                    if (cell.IsFlagged)
                    {
                        block.SetFlag();
                        continue;
                    }
                    if (cell.IsExposed)
                    {
                        if (cell.IsBomb && cell.IsFlipped)
                        {
                            block.SetBomb();
                        }
                        else if (cell.IsFlipped)
                        {
                            block.SetFlip();
                        }
                        else
                        {
                            block.SetNumber(cell.Number);
                        }
                    }
                    else
                    {
                        block.SetEmpty();
                    }
                }
            }
        }
        /// <summary>
        /// Set the mine remaining text on UI
        /// </summary>
        private static void SetMinesText()
        {
            MinesRemainingTextView.Text = (_gameEngine.GetBombsRemaining()).ToString();
        }
        /// <summary>
        /// Stop the chronometer
        /// </summary>
        private static void StopTimer()
        {
            Timer.Stop();
        }
        /// <summary>
        /// Start the chronometer
        /// </summary>
        private static void StartTimer()
        {
            Timer.Start();
        }
        /// <summary>
        /// Create and populate UI block table based on engine data
        /// </summary>
        private void PopulateTable()
        {
            var height = _gameEngine.GetHeight();
            var width = _gameEngine.GetWidth();
            _blocks = new Block[width, height];
            for (var row = 0; row < height; row++)
            {

                var mineRow = new TableRow(this);
                var mineRowParams = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent, 0.0f);

                for (var col = 0; col < width; col++)
                {
                    var block = new Block(this)
                    {
                        Row = row,
                        Col = col,
                        IsFlipped = false,
                        IsFlagged = false
                    };
                    _blocks[col, row] = block;
                    mineRow.AddView(_blocks[col, row]);
                }
                _mineTable.AddView(mineRow, mineRowParams);
            }
        }
        /// <summary>
        /// Runner thread that checks the current game state
        /// </summary>
        private void Runner()
        {
            while (_run)
            {
                switch (GameState)
                {
                    case GameState.Running:
                        continue;
                    case GameState.NotStarted:
                        continue;
                    default:
                        _run = false;
                        break;
                }
            }
            StopTimer();
            if (GameState == GameState.Win)
            {
                WinPopup();
            }
            else
            {
                UpdateTable();
                LossPopup();
            }
        }
        #endregion

    }
}