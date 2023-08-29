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
using Xamarin.Essentials;
using AppAdopción.ViewModels;

namespace AppAdopción.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicacionesPropiasPage : ContentPage
    {
        private PublicacionesPropiasViewModel viewModel;
        public PublicacionesPropiasPage()
        {
            InitializeComponent();

            viewModel = new PublicacionesPropiasViewModel(Navigation);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ObtenerPublicacionesPropias();
        }

        
    }
}