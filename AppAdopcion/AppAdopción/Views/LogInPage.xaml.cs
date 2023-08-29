using Acr.UserDialogs;
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
	public partial class LogInPage : ContentPage
	{
        public LogInPage ()
		{
			InitializeComponent ();
		}

        private async void logButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                //comprobar credenciales
                if (String.IsNullOrEmpty(emailEntry.Text) || String.IsNullOrEmpty(passEntry.Text))
                {
                    errormsg.Text = "Los campos son obligatorios";

                    return;
                }
                
                UserDialogs.Instance.ShowLoading("Iniciando Sesión...");
                await Task.Delay(100);

                var options = new RestClientOptions(App.APIUrl + "LoGin")
                {
                    MaxTimeout = -1
                };

                LogInParameters credenciales = new LogInParameters
                {
                    EmailUsuario = emailEntry.Text,
                    Password = passEntry.Text
                };

                var body = JsonConvert.SerializeObject(credenciales);

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);



                RestResponse response = await client.ExecuteAsync(request);

                UserDialogs.Instance.HideLoading();
                //mandar la clase usuario con las preferencias y obtenerlo en las páginas que se necesiten
                if (response.IsSuccessStatusCode)
                {
                    errormsg.Text = "";
                    Preferences.Clear();
                    Usuario usuariolog = JsonConvert.DeserializeObject<Usuario>(response.Content);
                    Preferences.Set("currentUser", JsonConvert.SerializeObject(usuariolog));

                    await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
                }
                else
                {
                    //comprobar si son válidos y poner error
                    errormsg.Text = "Email o contraseña incorrectos";
                    return;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: iniciar sesión, mensaje: " + ex.Message);
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "No se ha podido iniciar sesión error con el servidor", "Ok");
            }
			
        }

        private async void regButton_Clicked(object sender, EventArgs e)
        {
			App.Current.MainPage = new RegisterPage();
        }
    }
}