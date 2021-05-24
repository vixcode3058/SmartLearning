using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SmartLearning
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Quiz : ContentPage
    {
         List<string> learnedTopics;
         List<Question> questions;
        Random randomObject = new Random();

        int num;
        public Question currentQuestion;
        private Option rightAns;

        public Quiz()
        {
            InitializeComponent();
            GetQuestionsFromDatabase();
        }


        private async  void GenrateQuestions()
        {
           
            num = randomObject.Next(questions.Count());
            currentQuestion =  questions.ElementAt(num) ;
            if (currentQuestion.type == "single")
            {
                foreach (var item in currentQuestion.options)
                {
                    if (item.status == "true")
                    {
                        rightAns = item;
                    }
                }
                SL_quiz.BindingContext = currentQuestion;
            }
            else
            {
                //await Navigation.PushAsync(new MultipleChoiceQuestions(currentQuestion));
                GenrateQuestions();
            }
           

        }

        private void GetQuestionsFromDatabase()
        {
            questions = new List<Question>();
            if (File.Exists(Storage.GetLocalPath(App._questionFile)))
            {
                App._questionList = Storage.ReadListFromDatabase<Question>(App._questionFile);

            }
            else
            {
                var data = Storage.ReadXMLFromEmbeddedResource<Question>(App._questionFile);
                Storage.SerializeAndWriteList<Question>(data, App._questionFile);

                App._questionList = Storage.ReadListFromDatabase<Question>(App._questionFile);

            }

            learnedTopics = (from TopicModel in App._topicList where TopicModel.Learned.Contains("True") select TopicModel.Title).ToList();

            foreach (var item in learnedTopics)
            {
                var question=  (from q in App._questionList where q.title.Contains(item) select q).ToList();
                questions.AddRange(question);
            }

            Debug.Write("No. of Questions", questions.Count.ToString());
            if (questions.Count() >= 1)
            {
                GenrateQuestions();
            }

        }


        public async void LV_options_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ans = (sender as ListView).SelectedItem as Option;
            if (ans.status == "true")
            {
                await DisplayAlert("Right Answer", "You are doing great work.", "Continue");
                GenrateQuestions();
            }
            else
            {

                await DisplayAlert("Wrong Answer", "Right Anwer Is : "+ rightAns.option, "Continue");
                GenrateQuestions();

            }
        }
    }
}