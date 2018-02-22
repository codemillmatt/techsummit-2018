using Android.App;
using Android.Widget;
using Android.OS;

using System.Net.Http;
using System.Net.Http.Headers;
using HelloJokes.Core;


using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace HelloJokes
{
    [Activity(Label = "HelloJokes", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Jokes";

            var getJoke = FindViewById<Android.Widget.Button>(Resource.Id.getJoke);

            getJoke.Click +=  (sender, e) =>
            {
                var frag = new LandingPage().CreateFragment(this);
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.fragment_frame_layout, frag, "main");
                ft.Commit();
            };
        }
    }
}

