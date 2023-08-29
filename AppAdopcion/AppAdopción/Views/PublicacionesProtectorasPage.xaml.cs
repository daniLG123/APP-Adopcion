using AppAdopción.Models;
using AppAdopción.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAdopción.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PublicacionesProtectorasPage : ContentPage
	{
        private PublicacionesProtectorasViewModel viewModel;

        private List<string> filtros;
        private List<string> valorFiltro;

        public PublicacionesProtectorasPage(Usuario protectora)
        {
            InitializeComponent();

            viewModel = new PublicacionesProtectorasViewModel(protectora);
            BindingContext = viewModel;

            //Filtros de busqueda por tipo
            filtros = new List<string>
            {
                "Sin filtro",
                "Ciudad",
                "Especie",
                "Edad",
                "Tamaño",
                "Sexo"
            };
            filtroPicker.ItemsSource = filtros;

            filtroPicker.SelectedIndex = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ObtenerTodasPublicaciones();
        }

        //Mostrar opciones dependiendo de lo seleccionado en el anterior picker
        private void filtroPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.Tipofiltro = (string)filtroPicker.SelectedItem;
            if (filtroPicker.SelectedIndex == 0)
            {
                valorFiltroEntry.IsVisible = false;
                valorFiltroPicker.IsVisible = false;
                valorFiltroEntry.Text = string.Empty;
                viewModel.AplicarFiltro();



            }
            else if (filtroPicker.SelectedIndex == 1)
            {
                valorFiltroEntry.IsVisible = true;
                valorFiltroPicker.IsVisible = false;

                valorFiltroEntry.Text = string.Empty;

            }
            else if (filtroPicker.SelectedIndex == 2)
            {
                valorFiltroEntry.IsVisible = true;
                valorFiltroPicker.IsVisible = false;
                valorFiltroEntry.Text = string.Empty;
            }
            else if (filtroPicker.SelectedIndex == 3)
            {
                valorFiltroEntry.IsVisible = false;
                valorFiltroPicker.IsVisible = true;
                valorFiltroEntry.Text = string.Empty;

                valorFiltro = new List<string>
                {
                    "0-5 Años",
                    "6-10 Años",
                    "11-15 Años",
                    "+15 Años"
                };

                valorFiltroPicker.ItemsSource = valorFiltro;
            }
            else if (filtroPicker.SelectedIndex == 4)
            {
                valorFiltroEntry.IsVisible = false;
                valorFiltroPicker.IsVisible = true;
                valorFiltroEntry.Text = string.Empty;

                valorFiltro = new List<string>
                {
                    "Pequeño",
                    "Mediano",
                    "Grande"
                };
                valorFiltroPicker.ItemsSource = valorFiltro;
            }
            else if (filtroPicker.SelectedIndex == 5)
            {
                valorFiltroEntry.IsVisible = false;
                valorFiltroPicker.IsVisible = true;
                valorFiltroEntry.Text = string.Empty;

                valorFiltro = new List<string>
                {
                    "Macho",
                    "Hembra"
                };
                valorFiltroPicker.ItemsSource = valorFiltro;
            }
        }

        private void valorFiltroPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.Filtro = (string)valorFiltroPicker.SelectedItem;
        }

        private void valorFiltroEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Filtro = valorFiltroEntry.Text;
        }
    }
}