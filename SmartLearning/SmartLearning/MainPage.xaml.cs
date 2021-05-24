using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartLearning
{
    public partial class MainPage : ContentPage
    {
         
        public MainPage()
        {
            InitializeComponent();
            LV_mainlist.ItemsSource = null;
            LoadData();
            
        }

        private  void LoadData()
        {
            if (File.Exists(Storage.GetLocalPath(App._fileName)))
            {
                App._topicList = Storage.ReadListFromDatabase<TopicModel>(App._fileName);


            }
            else
            {

                var data =  Storage.ReadXMLFromEmbeddedResource<TopicModel>(App._fileName);
                Storage.SerializeAndWriteList<TopicModel>(data, App._fileName);

                App._topicList =  Storage.ReadListFromDatabase<TopicModel>(App._fileName);

            }
           var mainTopics = from TopicModel in App._topicList where TopicModel.Id % 1000 == 0 select TopicModel;
            LV_mainlist.ItemsSource = mainTopics;


        }
        
        private async void LV_mainlist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var topicInfo = (TopicModel) e.SelectedItem;

             await Navigation.PushAsync(new TopicDetails(topicInfo));

        }

        private async void TB_quiz_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new showQuestions());

        }

        private async void TB_result_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultPage());
        }
    }
}
