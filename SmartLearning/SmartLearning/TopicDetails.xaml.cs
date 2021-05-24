using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLearning
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TopicDetails : ContentPage
	{
        TopicModel topic;
		public TopicDetails (TopicModel topicInfo)
		{
			InitializeComponent ();
            topic = topicInfo;
            BindingContext = topic;
            SW_learned.IsToggled = Convert.ToBoolean(topic.Learned);

            DisplaySubGroups();
        }


        private void DisplaySubGroups()
        {
             LV_subTopics.ItemsSource = from TopicModel in App._topicList where (TopicModel.Id > topic.Id && TopicModel.Id < topic.Id + 999) select TopicModel;
            LV_subTopics.IsVisible = true;
            }

        private void LV_subTopics_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           SL_topicDetail.BindingContext = e.SelectedItem as TopicModel;
        }

        private void SW_learned_Toggled(object sender, ToggledEventArgs e)
        {
            var updateValue = App._topicList.FirstOrDefault(c => c.Title == topic.Title);
            updateValue.Learned = Convert.ToString(e.Value);

            Storage.SerializeAndWriteList(App._topicList, App._fileName);

        }
    }



    
}