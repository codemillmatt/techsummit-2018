using Android.App;
using Android.Widget;
using Android.OS;

using System.Net.Http;
using System.Net.Http.Headers;
using HelloJokes.Core;

namespace HelloJokes
{
    [Activity(Label = "HelloJokes", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var getJoke = FindViewById<Button>(Resource.Id.getJoke);
            var jokeText = FindViewById<TextView>(Resource.Id.joke);

            getJoke.Click += async (sender, e) =>
            {
                var jokeService = new JokeService();
                var theJoke = await jokeService.GetJoke();
                jokeText.Text = theJoke.Joke;
            };
        }
    }
}

