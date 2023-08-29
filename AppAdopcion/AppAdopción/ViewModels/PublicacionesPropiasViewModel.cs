using AppAdopción.Models;
using AppAdopción.Views;
using Newtonsoft.Json;
using RestSharp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAdopción.ViewModels
{
    class PublicacionesPropiasViewModel : INotifyPropertyChanged
    {
        private Usuario currentuser;
        private ObservableCollection<Publicacion> publicaciones;
        private INavigation _navigation;
        public ICommand ModifyCommand { private set; get; }
        public ICommand DeleteCommand { private set; get; }
        public ObservableCollection<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { SetProperty(ref publicaciones, value); }
        }

        public PublicacionesPropiasViewModel()
        {
            Publicaciones = new ObservableCollection<Publicacion>();

            ModifyCommand = new Command<Publicacion>(EditarPublicacion);
            DeleteCommand = new Command<Publicacion>(EliminarPublicacion);
        }

        public PublicacionesPropiasViewModel(INavigation navigation)
        {
            Publicaciones = new ObservableCollection<Publicacion>();

            ModifyCommand = new Command<Publicacion>(EditarPublicacion);
            DeleteCommand = new Command<Publicacion>(EliminarPublicacion);
            _navigation = navigation;
        }

        private async void EditarPublicacion(Publicacion publi)
        {
            await _navigation.PushAsync(new ModificarPublicacionPage(publi));

            //await PopupNavigation.Instance.PushAsync(new ModificarPublicacionPage(publi));
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ModificarPublicacionPage(publi)));
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ModificarPublicacionPage(publi)));
        }

        private async void EliminarPublicacion(Publicacion publi)
        {
            var respuesta = await App.Current.MainPage.DisplayAlert("Confirmar", "¿Desea eliminar esta publicación?", "Confirmar", "Cancelar");
            
            if(respuesta)
            {
                await BorrarPublicacion(publi);
                Publicaciones.Remove(publi);
            }            
        }

        public async Task BorrarPublicacion(Publicacion publi)
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/" + publi.PublicacionId)
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Delete;

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Borrado correcto " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString());
                    /*Publicaciones.Clear();
                    List<Publicacion> publicacioneslista = JsonConvert.DeserializeObject<List<Publicacion>>(response.Content);
                    Publicaciones = new ObservableCollection<Publicacion>(publicacioneslista);*/
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


        public async Task ObtenerPublicacionesPropias()
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/user/" + currentuser.UsuarioId)
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
