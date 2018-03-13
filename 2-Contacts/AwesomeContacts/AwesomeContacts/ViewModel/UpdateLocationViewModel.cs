using AwesomeContacts.Resources;
using AwesomeContacts.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
{
    public class UpdateLocationViewModel : ViewModelBase
    {

        public UpdateLocationViewModel()
        {
            UpdateLocationCommand = new Command(async () => await ExecuteUpdateLocationCommand());
        }

        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        public ICommand UpdateLocationCommand { get; }

        async Task ExecuteUpdateLocationCommand()
        {
            if (IsBusy)
                return;



            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get location: " + ex);
                UpdateMessage = string.Empty;
                await Dialogs.AlertAsync(null, AppResources.UpdateLocationError, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
