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
                //3. Check connectivity
                if (!await CheckConnectivityAsync())
                    return;

                //4. Login
                var authResult = await AuthenticationService.Login();
                if (authResult == null)
                    return;

                //5. Update UI
                IsBusy = true;

                UpdateMessage = AppResources.UpdatingLocation;

                //6. Get current location
                var position = await Geolocation.GetCurrentPositionAsync();

                if (position == null)
                    throw new Exception("Unable to get location.");

                CurrentLocation = $"{position.Latitude}, {position.Longitude}";

                //7. Get address from current location
                UpdateMessage = AppResources.UpdateLocationGeocoding;

                var address = await Geolocation.GetAddressAsync(position);

                if (address != null)
                    CurrentLocation = $"{address.Locality}, {address.AdminArea ?? string.Empty} {address.CountryCode}";

                UpdateMessage = AppResources.UpdateLocationBackend;

                //8. Update CosmosDB with current location
                await DataService.UpdateLocationAsync(position, address, authResult.AccessToken);

                UpdateMessage = AppResources.UpdatingLocationUpdated;
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
