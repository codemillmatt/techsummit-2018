using System;
using Xamarin.Forms;
namespace JokeForms.Core
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new MainJokePage());
        }

    }
}
