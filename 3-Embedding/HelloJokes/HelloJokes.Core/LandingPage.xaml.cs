using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HelloJokes.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LandingPage : ContentPage
	{
		public LandingPage ()
		{
			InitializeComponent ();

            okButton.Clicked += OkButton_Clicked;
		}

        private async void OkButton_Clicked(object sender, EventArgs e)
        {
            var jokeService = new JokeService();
            jokeText.Text = (await jokeService.GetJoke()).Joke;
        }
    }
}