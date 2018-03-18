using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
namespace JokeForms.Core
{
    public class MainJokeViewModel : INotifyPropertyChanged
    {
        public MainJokeViewModel()
        {
            GetJokeCommand = new Command(async () => await ExecuteGetJoke());
            Joke = "From VM";
        }

        public ICommand GetJokeCommand { get; set; }
        private async Task ExecuteGetJoke()
        {
            var jokeService = new JokeService();

            if (string.IsNullOrEmpty(SearchTerm))
            {
                var theJoke = await jokeService.GetJoke();

                Joke = theJoke.Joke;
            }
            else
            {
                var jokeResults = await jokeService.GetJoke(SearchTerm);

                Joke = jokeResults.Results.FirstOrDefault().Joke;
            }
        }

        string _joke;
        public string Joke
        {
            get => _joke;
            set
            {
                if (_joke == value)
                    return;

                _joke = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(
                    nameof(Joke)));
            }
        }

        string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm == value)
                    return;

                _searchTerm = value;
                PropertyChanged?.Invoke(this,
                                        new PropertyChangedEventArgs(nameof(SearchTerm)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
