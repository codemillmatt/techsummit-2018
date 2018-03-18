using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace JokeForms.Core
{
    public partial class MainJokePage : ContentPage
    {
        public MainJokePage()
        {
            InitializeComponent();

            getJoke.Clicked += async (sender, args) =>
            {
                var jokeService = new JokeService();

                var theJoke = await jokeService.GetJoke();

                jokeText.Text = theJoke.Joke;
            };
        }
    }
}
