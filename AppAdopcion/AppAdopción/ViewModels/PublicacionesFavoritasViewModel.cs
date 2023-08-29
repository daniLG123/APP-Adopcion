using AppAdopción.Models;
using AppAdopción.Views;
using Newtonsoft.Json;
using RestSharp;
using Rg.Plugins.Popup.Services;
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
    public class PublicacionesFavoritasViewModel : INotifyPropertyChanged
    {
        private Usuario currentuser;

        private INavigation _navigation;

        private ObservableCollection<Publicacion> publicaciones;
        public ICommand MostrarCommand { private set; get; }
        public ICommand FavCommand { private set; get; }

        public delegate void PopupClosedEventHandler();

        public event PopupClosedEventHandler PopupClosed;

        public ObservableCollection<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { SetProperty(ref publicaciones, value); }
        }
        public PublicacionesFavoritasViewModel()
        {
            Publicaciones = new ObservableCollection<Publicacion>();

            MostrarCommand = new Command<Publicacion>(MostrarPublicacion);
        }

        

        private async void MostrarPublicacion(Publicacion publi)
        {
            await PopupNavigation.Instance.PushAsync(new PublicacionFavoritaPopUp(publi, this));
            PopupClosed += () => OnPopupClosed();
        }

        private async void OnPopupClosed()
        {
            await ObtenerTodasPublicaciones();

        }

        public void RaisePopupClosedEvent()
        {
            PopupClosed?.Invoke();
        }

        public async Task ObtenerTodasPublicaciones()
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/fav/" + currentuser.UsuarioId)
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Get;

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Publicaciones.Clear();
                    List<Publicacion> publicacioneslista = JsonConvert.DeserializeObject<List<Publicacion>>(response.Content);
                    Publicaciones = new ObservableCollection<Publicacion>(publicacioneslista);
                }
                else
                {
                    Console.WriteLine("Error al obtener publicaciones " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener publicaciones, no se ha podido procesar la solicitud | " + ex.Message);
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
