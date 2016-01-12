using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidSweeper
{
    [Activity(Label = "AndroidSweeper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var playButton = FindViewById<Button>(Resource.Id.PlayButton);
            playButton.Click += (sender, e) => {
                var game = new Intent(this, typeof(Game));
                StartActivity(game);
            };

            var aboutButton = FindViewById<Button>(Resource.Id.AboutButton);
            aboutButton.Click += (sender, e) => {
                var about = new Intent(this, typeof(About));
                StartActivity(about);
            };

        }
    }
}

