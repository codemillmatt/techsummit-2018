using HelloJokes.Core;
using System;

using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace HelloJokes.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            getJoke.TouchUpInside += (sender, e) =>
            {
                //var jokeService = new JokeService();

                //jokeText.Text = (await jokeService.GetJoke()).Joke;

                var landingVC = new LandingPage().CreateViewController();

                NavigationController.PushViewController(landingVC, true);
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}