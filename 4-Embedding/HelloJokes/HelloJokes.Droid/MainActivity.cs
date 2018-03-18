using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;

using System;
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
    [Activity(Label = "HelloJokes", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment_frame_layout, new MainFragment(), "main");

            //8. Create fragment
            //var landingFragment = new LandingPage().CreateFragment(this);

            //9. Replace fragment
            //ft.Replace(Resource.Id.fragment_frame_layout, landingFragment, "main");

            ft.Commit();

        }
    }

    public class MainFragment : Fragment
    {
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.JokeFragment, container, false);

            var getJoke = view.FindViewById<Android.Widget.Button>(Resource.Id.getJoke);

            getJoke.Click += async (sender, e) =>
            {
                var jokeText = view.FindViewById<Android.Widget.TextView>(Resource.Id.joke);

                var jokeService = new JokeService();
                jokeText.Text = (await jokeService.GetJoke()).Joke;
            };

            return view;
        }

    }

}

