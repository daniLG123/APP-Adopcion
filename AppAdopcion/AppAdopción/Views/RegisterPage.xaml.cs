using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAdopción.Models;
using Acr.UserDialogs;

namespace AppAdopción.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();
		}

        private void gotologinButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LogInPage();
        }

        private async void registButton_Clicked(object sender, EventArgs e)
        {

            if(string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(usuarioEntry.Text) || string.IsNullOrEmpty(passEntry.Text) || string.IsNullOrEmpty(movilEntry.Text) || string.IsNullOrEmpty(ciudadEntry.Text) || string.IsNullOrEmpty(repPassEntry.Text))
            {
                errormsg.Text = "Todos los campos son obligatorios";
                return;
            }

            if(passEntry.Text != repPassEntry.Text)
            {
                errormsg.Text = "Las contraseñas deben de ser identicas";
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Registrando usuario...");
                await Task.Delay(100);

                var options = new RestClientOptions(App.APIUrl + "usuario")
                {
                    MaxTimeout = -1
                };

                string protectora;

                if (protectoraSwitch.IsToggled)
                    protectora = "S";
                else
                    protectora = "N";

                Usuario nuevoUsuario = new Usuario
                {
                    Email = emailEntry.Text,
                    Username = usuarioEntry.Text,
                    Password = passEntry.Text,
                    EsProtectora = protectora,
                    Telefono = movilEntry.Text,
                    Ciudad = ciudadEntry.Text
                };


                var body = JsonConvert.SerializeObject(nuevoUsuario);

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = await client.ExecuteAsync(request);

                UserDialogs.Instance.HideLoading();
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Cuenta", "Se ha creado la cuenta de forma exitosa", "Ok");

                    App.Current.MainPage = new LogInPage();
                }
                else
                    await DisplayAlert("Error", "Error al registrarse " + "StatusCode: " + response.StatusCode.ToString() + " | " + response.Content.ToString() , "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: añadir usuario, mensaje: " + ex.Message);
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "Error al registrarse, no se ha podido procesar la solicitud | " + ex.Message, "Ok");
            }
        }
    }
}