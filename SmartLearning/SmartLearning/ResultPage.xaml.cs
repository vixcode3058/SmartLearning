using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLearning
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultPage : ContentPage
	{
		public ResultPage ()
		{
			InitializeComponent ();
            ShowResult();
		}

        private async void ShowResult()
        {
            LV_results.ItemsSource = null;
            try
            {
                LV_results.ItemsSource = await App.Database.getIetmsAsync();

            }
            catch (Exception)
            {

                await this.DisplayAlert("Alert!", "There is no result to show.", "Ok");
                await Navigation.PopAsync();

            }
        }

    }
}