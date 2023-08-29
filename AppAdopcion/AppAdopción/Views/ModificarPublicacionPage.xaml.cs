using Acr.UserDialogs;
using AppAdopción.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAdopción.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModificarPublicacionPage : ContentPage
	{
        private byte[] foto = null;
        public Publicacion currentPublicacion;
        public ModificarPublicacionPage (Publicacion publicacion)
		{
			InitializeComponent ();

            currentPublicacion = publicacion;
            SetDatosPublicacion();
            
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

        private void SetDatosPublicacion()
        {
            descPubli.Text = currentPublicacion.Descripcion;
            ciudadAnim.Text = currentPublicacion.Ciudad;
            nombreAnim.Text = currentPublicacion.NombreAnimal;
            especieAnim.Text = currentPublicacion.EspecieAnimal;
            edadAnim.Text = currentPublicacion.EdadAnimal.ToString();
            foto = currentPublicacion.FotoAnimal;

            tamAnim.SelectedItem = currentPublicacion.TamanyoAnimal;

            if (currentPublicacion.SexoAnimal == "Macho")
            {
                maleRadioButton.IsChecked = true;
                femaleRadioButton.IsChecked = false;
            }
            else
            {
                maleRadioButton.IsChecked = false;
                femaleRadioButton.IsChecked = true;
            }

            imagenAnimal.Source = currentPublicacion.Foto;
            //imagenAnimal.Source = ImageSource.FromStream(() => new MemoryStream(foto));
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

                currentPublicacion.Descripcion = descPubli.Text;
                currentPublicacion.Ciudad = ciudadAnim.Text;
                currentPublicacion.NombreAnimal = nombreAnim.Text;
                currentPublicacion.EspecieAnimal = especieAnim.Text;
                currentPublicacion.EdadAnimal = int.Parse(edadAnim.Text);
                currentPublicacion.TamanyoAnimal = tamAnim.SelectedItem.ToString();
                currentPublicacion.SexoAnimal = sexo;
                currentPublicacion.FotoAnimal = foto;

                Publicacion2 publi2 = new Publicacion2()
                {
                    PublicacionId = currentPublicacion.PublicacionId,
                    UsuarioId = currentPublicacion.UsuarioId,
                    Descripcion = descPubli.Text,
                    Ciudad = ciudadAnim.Text,
                    NombreAnimal = nombreAnim.Text,
                    EspecieAnimal = especieAnim.Text,
                    EdadAnimal = int.Parse(edadAnim.Text),
                    TamanyoAnimal = tamAnim.SelectedItem.ToString(),
                    SexoAnimal = sexo,
                    FotoAnimal = foto,
                };


                var body = JsonConvert.SerializeObject(publi2);

                var options = new RestClientOptions(App.APIUrl + "publicacion")
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Put;
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
                    await DisplayAlert("Error", "Error al Modificar publicación " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString(), "Ok");
            }
            catch (Exception ex)
            {

                Console.WriteLine("exception: Modificar publicacion, mensaje: " + ex.Message);
                await DisplayAlert("Error", "Error al Modificar publicacion, no se ha podido procesar la solicitud || mensaje: " + ex.Message, "Ok");
            }
        }

        private async void cancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}