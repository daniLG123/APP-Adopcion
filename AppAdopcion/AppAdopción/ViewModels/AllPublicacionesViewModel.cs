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
    class AllPublicacionesViewModel : INotifyPropertyChanged
    {
        private Usuario currentuser;
        private string filtro;
        private string tipofiltro;

        private INavigation _navigation;

        private ObservableCollection<Publicacion> publicaciones;
        public ICommand MostrarCommand { private set; get; }
        public ICommand FavCommand { private set; get; }
        public ObservableCollection<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { SetProperty(ref publicaciones, value); }
        }

        public string Filtro
        {
            get { return filtro; }
            set
            {
                if (SetProperty(ref filtro, value))
                    AplicarFiltro();
            }
        }

        public string Tipofiltro
        {
            get { return tipofiltro; }
            set
            {
                SetProperty(ref tipofiltro, value);
            }
        }

        private ObservableCollection<Publicacion> publicacionesFiltradas;

        public ObservableCollection<Publicacion> PublicacionesFiltradas
        {
            get { return publicacionesFiltradas; }
            set { SetProperty(ref publicacionesFiltradas, value); }
        }

        public AllPublicacionesViewModel()
        {
            Publicaciones = new ObservableCollection<Publicacion>();
            PublicacionesFiltradas = new ObservableCollection<Publicacion>();

            MostrarCommand = new Command<Publicacion>(MostrarPublicacion);
        }

        public AllPublicacionesViewModel(INavigation navigation)
        {
            Publicaciones = new ObservableCollection<Publicacion>();
            PublicacionesFiltradas = new ObservableCollection<Publicacion>();

            MostrarCommand = new Command<Publicacion>(MostrarPublicacion);

            _navigation = navigation;

        }

        private async void MostrarPublicacion(Publicacion publi)
        {
            await PopupNavigation.Instance.PushAsync(new PublicacionPopUp(publi));
        }

        public void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(Tipofiltro) || string.IsNullOrWhiteSpace(Filtro) )
            {
                // Si no hay filtro, mostrar todas las publicaciones
                PublicacionesFiltradas = Publicaciones;
                return;
            }
            else if(tipofiltro.ToLower() == "sin filtro")
            {
                // Si no hay filtro es sin filtro, mostrar todas las publicaciones
                PublicacionesFiltradas = Publicaciones;
                return;
            }

            var filtroLower = Filtro.ToLower();
            var tipofiltroLower = Tipofiltro.ToLower();

            var publFiltradas = Publicaciones;

            if (tipofiltroLower == "ciudad")
            {
                publFiltradas = new ObservableCollection<Publicacion>(
                Publicaciones.Where(p => p.Ciudad.ToLower().Contains(filtroLower)));
            }
            else if (tipofiltroLower == "especie")
            {
                publFiltradas = new ObservableCollection<Publicacion>(
                Publicaciones.Where(p => p.EspecieAnimal.ToLower().Contains(filtroLower)));
            }
            else if (tipofiltroLower == "edad")
            {
                if(filtroLower.Contains("0-5"))
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.EdadAnimal >= 0 && p.EdadAnimal <= 5));
                }
                else if(filtroLower.Contains("6-10"))
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.EdadAnimal >= 6 && p.EdadAnimal <= 10));
                }
                else if (filtroLower.Contains("11-15"))
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.EdadAnimal >= 11 && p.EdadAnimal <= 15));
                }
                else if (filtroLower.Contains("+15"))
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.EdadAnimal >= 15)); 
                }
                
            }
            else if (tipofiltroLower == "tamaño")
            {
                if (filtroLower == "pequeño")
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.TamanyoAnimal.ToLower() == "pequeño"));
                }
                else if (filtroLower == "mediano")
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.TamanyoAnimal.ToLower() == "mediano"));
                }
                else if (filtroLower == "grande")
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.TamanyoAnimal.ToLower() == "grande"));
                }
                

            }
            else if (tipofiltroLower == "sexo")
            {
                if (filtroLower == "macho")
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.SexoAnimal.ToLower() == "macho"));
                }
                else if (filtroLower == "hembra")
                {
                    publFiltradas = new ObservableCollection<Publicacion>(
                    Publicaciones.Where(p => p.SexoAnimal.ToLower() == "hembra"));
                }
            }

            PublicacionesFiltradas = publFiltradas;
        }



        public async Task ObtenerTodasPublicaciones()
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/otros/" + currentuser.UsuarioId)
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
                    PublicacionesFiltradas.Clear();
                    List<Publicacion> publicacioneslista = JsonConvert.DeserializeObject<List<Publicacion>>(response.Content);
                    Publicaciones = new ObservableCollection<Publicacion>(publicacioneslista);
                    PublicacionesFiltradas = new ObservableCollection<Publicacion>(publicacioneslista);
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
