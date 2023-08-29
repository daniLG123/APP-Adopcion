using AppAdopción.Models;
using AppAdopción.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAdopción
{
    public partial class App : Application
    {
        public static string APIUrl = ""/*Direccion API*/;
        public App()
        {
            InitializeComponent();

            MainPage = new LogInPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
