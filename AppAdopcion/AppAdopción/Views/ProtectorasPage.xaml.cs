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
	public partial class ProtectorasPage : ContentPage
	{
        private ProtectorasViewModel viewModel;
        public ProtectorasPage ()
		{
			InitializeComponent ();
            viewModel = new ProtectorasViewModel(Navigation);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ObtenerProtectoras();
        }
    }
}