using AppAdopción.Models;
using Newtonsoft.Json;
using RestSharp;
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
	public partial class PerfilPage : ContentPage
	{
        Usuario currentuser;
        public PerfilPage ()
		{
			InitializeComponent ();
            currentuser = JsonConvert.DeserializeObject<Usuario>(Preferences.Get("currentUser", ""));

            if(currentuser != null)
            {
                usernameEntry.Text = currentuser.Username;
                emailEntry.Text = currentuser.Email;
                movilEntry.Text = currentuser.Telefono;
                passEntry.Text = currentuser.Password;
                ciudadEntry.Text = currentuser.Ciudad;
            }
            else
            {
                DisplayAlert("Error", "Error al cargar los datos del usuario", "Ok");
            }
        }

        private async void nuevapubliButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NuevaPublicacionPage());
        }

        private async void verpubliButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PublicacionesPropiasPage());
        }

        //Función para guardar los cambios de una edición de datos
        private async void editarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(usernameEntry.Text) || string.IsNullOrEmpty(passEntry.Text) || string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(movilEntry.Text))
                {

                    return;
                }
                currentuser.Username = usernameEntry.Text;
                currentuser.Email = emailEntry.Text;
                currentuser.Telefono = movilEntry.Text;
                currentuser.Password = passEntry.Text;
                currentuser.Ciudad = ciudadEntry.Text;


                var options = new RestClientOptions(App.APIUrl + "usuario")
                {
                    MaxTimeout = -1
                };

                var body = JsonConvert.SerializeObject(currentuser);

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Put;

                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Preferences.Set("currentUser", JsonConvert.SerializeObject(currentuser));
                    await DisplayAlert("Guardado", "Se ha actualizado correctamente", "Ok");
                }
                else
                    await DisplayAlert("Error", "Error al actualizar el perfil " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: añadir publicacion, mensaje: " + ex.Message);
                await DisplayAlert("Error", "Error al actualizar perfil, no se ha podido procesar la solicitud || mensaje: " + ex.Message, "Ok");
            }

        }
    }
}