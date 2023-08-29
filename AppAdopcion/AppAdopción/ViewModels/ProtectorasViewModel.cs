using AppAdopción.Models;
using AppAdopción.Views;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAdopción.ViewModels
{
    class ProtectorasViewModel : INotifyPropertyChanged
    {
        private Usuario currentuser;
        private ObservableCollection<Usuario> protectoras;
        private INavigation _navigation;
        public ICommand AccederCommand { private set; get; }
        public ObservableCollection<Usuario> Protectoras
        {
            get { return protectoras; }
            set { SetProperty(ref protectoras, value); }
        }

        public ProtectorasViewModel()
        {
            Protectoras = new ObservableCollection<Usuario>();

            AccederCommand = new Command<Usuario>(AccederListaPublicacionesProtectora);
        }

        public ProtectorasViewModel(INavigation navigation)
        {
            Protectoras = new ObservableCollection<Usuario>();

            AccederCommand = new Command<Usuario>(AccederListaPublicacionesProtectora);

            _navigation = navigation;
        }

        private async void AccederListaPublicacionesProtectora(Usuario us)
        {
            await _navigation.PushAsync(new PublicacionesProtectorasPage(us));

            //await PopupNavigation.Instance.PushAsync(new ModificarPublicacionPage(publi));
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ModificarPublicacionPage(publi)));
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ModificarPublicacionPage(publi)));
        }
        public async Task ObtenerProtectoras()
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "usuario/protectoras/" + currentuser.UsuarioId)
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Get;

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Protectoras.Clear();
                    List<Usuario> protectoraslista = JsonConvert.DeserializeObject<List<Usuario>>(response.Content);
                    Protectoras = new ObservableCollection<Usuario>(protectoraslista);
                }
                else
                {
                    Console.WriteLine("Error al obtener protectoras " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener protectoras, no se ha podido procesar la solicitud | " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            bool forceUpdate = false)
        {
            if (!forceUpdate && EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
