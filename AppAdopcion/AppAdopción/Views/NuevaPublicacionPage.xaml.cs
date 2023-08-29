using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using AppAdopción.Models;
using Xamarin.Forms.PlatformConfiguration;
using Acr.UserDialogs;

namespace AppAdopción.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NuevaPublicacionPage : ContentPage
	{
        private byte[] foto = null;
        Usuario currentuser;
		public NuevaPublicacionPage ()
		{
			InitializeComponent ();
            currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));
            tamAnim.SelectedIndex = 1;
		}

        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                foto = new byte[stream.Length];
                await stream.ReadAsync(foto, 0, foto.Length);

                imagenAnimal.Source = ImageSource.FromStream(() => new MemoryStream(foto));
                imagenAnimal.IsVisible = true;

            }
        }

        private async void publicarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (foto == null || String.IsNullOrEmpty(descPubli.Text) || String.IsNullOrEmpty(ciudadAnim.Text) || String.IsNullOrEmpty(nombreAnim.Text) || String.IsNullOrEmpty(especieAnim.Text))
                {
                    errormsg.Text = "Todos los campos son obligatorios";
                    return;
                }
                else
                    errormsg.Text = string.Empty;

                string sexo = "";
                if (maleRadioButton.IsChecked)
                {
                    sexo = "Macho";
                }
                else
                {
                    sexo = "Hembra";
                }
                var publi = new Publicacion2
                {
                    UsuarioId = currentuser.UsuarioId,
                    Descripcion = descPubli.Text,
                    Ciudad = ciudadAnim.Text,
                    NombreAnimal = nombreAnim.Text,
                    EspecieAnimal = especieAnim.Text,
                    EdadAnimal = int.Parse(edadAnim.Text),
                    TamanyoAnimal = tamAnim.SelectedItem.ToString(),
                    SexoAnimal = sexo,
                    FotoAnimal = foto
                };

                var body = JsonConvert.SerializeObject(publi);                

                var options = new RestClientOptions(App.APIUrl + "publicacion")
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = await client.ExecuteAsync(request);

                UserDialogs.Instance.HideLoading();
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Guardado", "Se ha guardado correctamente", "Ok");
                    await Navigation.PopAsync();
                }
                else
                    await DisplayAlert("Error", "Error al subir publicación " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: añadir publicacion, mensaje: " + ex.Message);
                await DisplayAlert("Error", "Error al subir publicacion, no se ha podido procesar la solicitud || mensaje: " + ex.Message, "Ok");
            }
        }
    }
}