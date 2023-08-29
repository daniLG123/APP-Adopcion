using AppAdopción.Models;
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
	public partial class MainPage : FlyoutPage
	{
		public MainPage ()
		{
			InitializeComponent();

			flyout.listview.ItemSelected += OnSelectedItem;
		}

		private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as FlyoutItemPage;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
				flyout.listview.SelectedItem = null;
				IsPresented = false;
			}

            /*var item = e.SelectedItem as FlyoutItemPage;

            if (item != null)
            {
                // create the new page
                var newpage = ((Page)Activator.CreateInstance(item.TargetPage));

                // get the navigation page
                var nav = (NavigationPage)Detail;

                // get the current displayed page
                var page = (ContentPage)nav.CurrentPage;

                // navigate to the new page
                page.Navigation.PushAsync(newpage);

                // hide the flyout
                this.IsPresented = false;
            }*/
        }
	}
}