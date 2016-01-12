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
            var playbutton = FindViewById<Button>(Resource.Id.PlayButton);
            playbutton.Click += (sender, e) => {
                var game = new Intent(this, typeof(Game));
                StartActivity(game);
            };
        }
    }
}

