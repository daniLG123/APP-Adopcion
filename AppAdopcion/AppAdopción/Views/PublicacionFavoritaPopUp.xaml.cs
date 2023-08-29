using AppAdopción.Models;
using AppAdopción.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAdopción.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicacionFavoritaPopUp : PopupPage
    {
        Publicacion publicacion;
        Usuario currentuser;
        bool isfavorito = false;

        PublicacionesFavoritasViewModel favviewmodel;
        public PublicacionFavoritaPopUp(Publicacion publi, PublicacionesFavoritasViewModel viewmodel)
        {
            InitializeComponent();

            publicacion = publi;
            favviewmodel = viewmodel;
            isFav();


            imagen.Source = publicacion.Foto;
            nombre.Text = publicacion.NombreAnimal;
            descripcion.Text = publicacion.Descripcion;
            fecha.Text = publicacion.FechaPublicacion.ToString("dd-MM-yyyy");
            tamanyo.Text = publicacion.TamanyoAnimal;
            edad.Text = publicacion.EdadAnimal + " Años";
            sexo.Text = publicacion.SexoAnimal;
            ciudad.Text = publicacion.Ciudad;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            favviewmodel?.RaisePopupClosedEvent();
        }

        private async void Volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        //Comprobar si la publicación ya está en favoritos
        private async void isFav()
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/fav/" + currentuser.UsuarioId + "/" + publicacion.PublicacionId)
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Get;

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    isfavorito = JsonConvert.DeserializeObject<bool>(response.Content);

                    if (isfavorito)
                    {
                        FavButton.Text = "Quitar Favorito";
                        FavButton.BackgroundColor = Color.Firebrick;
                    }
                    else
                    {
                        FavButton.Text = "Añadir Favorito";
                        FavButton.BackgroundColor = Color.Gray;
                    }

                }
                else
                {
                    Console.WriteLine("Error al obtener publicaciones fav " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener publicaciones fav, no se ha podido procesar la solicitud | " + ex.Message);
            }
        }

        //Guardar en favoritos
        private async void FavButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

                var options = new RestClientOptions(App.APIUrl + "publicacion/fav/" + currentuser.UsuarioId + "/" + publicacion.PublicacionId)
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();

                if (isfavorito)
                {
                    request.Method = Method.Delete;
                }
                else
                {
                    request.Method = Method.Post;
                }


                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    isfavorito = !isfavorito;
                    if (isfavorito)
                    {
                        FavButton.Text = "Quitar Favorito";
                        FavButton.BackgroundColor = Color.Firebrick;
                    }
                    else
                    {
                        FavButton.Text = "Añadir Favorito";
                        FavButton.BackgroundColor = Color.Gray;
                    }
                }
                else
                {
                    Console.WriteLine("Error al asociar favorito " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al asociar favorito, no se ha podido procesar la solicitud | " + ex.Message);
            }
        }
    }
}