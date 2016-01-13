using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;

namespace AndroidSweeper
{
    [Activity(Label = "AndroidSweeper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private EditText _mineEditText;
        private EditText _heightEditText;
        private EditText _widthEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Grab UI Edit text feild settings
            _mineEditText = FindViewById<EditText>(Resource.Id.editTextMines);
            _heightEditText = FindViewById<EditText>(Resource.Id.editTextHeight);
            _widthEditText = FindViewById<EditText>(Resource.Id.editTextWidth);

            // Get our button from the layout resource,
            // and attach an event to it
            var playButton = FindViewById<Button>(Resource.Id.PlayButton);
            playButton.Click += (sender, e) =>
            {
                var game = new Intent(this, typeof(Game));
                game.PutExtra("mines", _mineEditText.Text);
                game.PutExtra("height", _heightEditText.Text);
                game.PutExtra("width", _widthEditText.Text);
                StartActivity(game);
            };

            var aboutButton = FindViewById<Button>(Resource.Id.AboutButton);
            aboutButton.Click += (sender, e) =>
            {
                var about = new Intent(this, typeof(About));
                StartActivity(about);
            };

            var difficultyRadioGroup = FindViewById<RadioGroup>(Resource.Id.DifficultyRadioGroup);
            difficultyRadioGroup.CheckedChange += (sender, e) =>
            {
                _mineEditText.Enabled = false;
                _heightEditText.Enabled = false;
                _widthEditText.Enabled = false;
                switch (e.CheckedId)
                {
                    case Resource.Id.EasyRadioButton:
                        _mineEditText.Text = "5";
                        _heightEditText.Text = "11";
                        _widthEditText.Text = "6";
                        break;
                    case Resource.Id.MediumRadioButton:
                        _mineEditText.Text = "10";
                        _heightEditText.Text = "11";
                        _widthEditText.Text = "7";
                        break;
                    case Resource.Id.HardRadioButton:
                        _mineEditText.Text = "15";
                        _heightEditText.Text = "20";
                        _widthEditText.Text = "20";
                        break;
                    default:
                        _mineEditText.Enabled = true;
                        _heightEditText.Enabled = true;
                        _widthEditText.Enabled = true;
                        break;


                }
            };
        }
    }
}

