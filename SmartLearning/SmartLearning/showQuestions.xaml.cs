using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLearning
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class showQuestions : ContentPage
    {
        List<string> learnedTopics;
        List<Question> questions;
        Random randomObject = new Random();

        int num;
        public Question currentQuestion;
        private List<string> rightAns;


        List<string> checkedAns;
        private List<Item> list;

        public Result result;

        public showQuestions()
        {
            InitializeComponent();
            GetQuestionsFromDatabase();

        }

        private async void GetQuestionsFromDatabase()
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
                var question = (from q in App._questionList where q.title.Contains(item) select q).ToList();
                questions.AddRange(question);
            }

            Debug.Write("No. of Questions", questions.Count.ToString());
            if (questions.Count() >= 1)
            {

                GenrateQuestions();
            }
            else
            {
                await DisplayAlert("Information", "Please Learn at least one topic to test your knowledge ", "Continue");
                await Navigation.PopAsync();
            }

        }

        public void GenrateQuestions()
        {
            rightAns = new List<string>();
            checkedAns = new List<string>();
            result = new Result();
            List<Item> itemSource = new List<Item>();

            // this statement get the question rendomly everytime
            num = randomObject.Next(questions.Count());
            currentQuestion = questions.ElementAt(num);
            LB_question.Text = currentQuestion.question;

            // This statment save the title of the question to show in the result
            result.topicName = currentQuestion.title;

            // This foreach loop add the options in the listview as an ITEM class so that we can implement the multiple selections and retrive the selected options
            foreach (var item in currentQuestion.options)
            {
                var setImage = new Image { Source= "unchecked.png" };
                itemSource.Add(new Item { image= setImage, option= item.option, status= item.status});
            }
            LV_options.ItemsSource = itemSource ;
            LV_options.ItemTapped += Tapped;
            foreach (var item in currentQuestion.options)
            {
                if (item.status == "true")
                {
                    rightAns.Add(item.option);
                }

            }
        }

        void Tapped(object sender, ItemTappedEventArgs args)
        {
            var listView = sender as ListView;
            var selectedItem = args.Item as Item;
            if (selectedItem.TextColor.Equals(Color.Black))
            {
                var setImage = new Image { Source = "checked.png" };


                selectedItem.TextColor = Color.Green;
                //selectedItem.image.Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromResource("checked.png") : ImageSource.FromFile("Images/checked.png");
                selectedItem.image = setImage;

                selectedItem.OnPropertyChanged();
                list = listView.ItemsSource as List<Item>;
               
            }
            else
            {
                var setImage = new Image { Source = "unchecked.png" };

                selectedItem.TextColor = Color.Black;
                //selectedItem.image.Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromResource("unchecked.png") : ImageSource.FromFile("Images/unchecked.png");

                selectedItem.image = setImage;

                selectedItem.OnPropertyChanged();
                list = listView.ItemsSource as List<Item>;

            }


        }

        private async void BT_submit_Clicked(object sender, EventArgs e)
        {
            int counter = 0;
            string answer = null;

            foreach (var item in list)
            {
                if (item.TextColor.Equals(Color.Green))
                {
                    checkedAns.Add(item.option);
                }
            }


            if (rightAns.Count == checkedAns.Count)
            {
                foreach (var item in rightAns)
                {
                    foreach (var i in checkedAns)
                    {
                        if (i.Equals(item))
                        {
                            counter++;

                        }
                    }
                }
                if (counter == rightAns.Count)
                {
                    await DisplayAlert("Right Answer", "You are doing great ", "Continue");
                    var count = result.correctAnswer;
                    result.correctAnswer = count + 1;
                    result.lastTimeAnswer = "Correct";
                    var list = await App.Database.getIetmsAsync();

                    foreach (var item in list)
                    {
                        if (item.topicName.Equals(result.topicName))
                        {
                            result.correctAnswer = result.correctAnswer + item.correctAnswer;
                            result.wrongAnswer = item.wrongAnswer;
                            await App.Database.DeleteRow(item);
                            break;

                        }
                    }

                    
                    await App.Database.SaveItemAsync(result);

                    GenrateQuestions();
                }
                else
                {
                    foreach (var item in rightAns)
                    {
                        answer = answer + item + " / ";
                    }
                    await DisplayAlert("Wrong Answer", "Right Answers are : " + answer, "Continue");

                    var count = result.wrongAnswer;
                    result.wrongAnswer = count + 1;
                    result.lastTimeAnswer = "Wrong";

                    var list = await App.Database.getIetmsAsync();

                    foreach (var item in list)
                    {
                        if (item.topicName.Equals(result.topicName))
                        {
                            result.wrongAnswer = result.wrongAnswer + item.wrongAnswer;
                            result.correctAnswer = item.correctAnswer;
                            await App.Database.DeleteRow(item);
                            break;

                        }
                    }

                    await App.Database.SaveItemAsync(result);

                    GenrateQuestions();
                }

            }
            else
            {
                foreach (var item in rightAns)
                {
                    answer = answer + item + " / ";
                }
                await DisplayAlert("Wrong Answer", "Right Answers are : " + answer, "Continue");
                var count = result.wrongAnswer;
                result.wrongAnswer = count + 1;
                result.lastTimeAnswer = "Wrong";

                var list = await App.Database.getIetmsAsync();

                foreach (var item in list)
                {
                    if (item.topicName.Equals(result.topicName))
                    {
                        result.wrongAnswer = result.wrongAnswer + item.wrongAnswer;
                        result.correctAnswer = item.correctAnswer;
                        await App.Database.DeleteRow(item);

                        break;
                    }
                }

                await App.Database.SaveItemAsync(result);


                GenrateQuestions();

            }

        }

        private async void TB_result_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultPage());
        }
    }

    public class Item : INotifyPropertyChanged
    {
        public Image image { get; set; }

        public string option { get; set; }
        public string status { get; set; }

        public Color TextColor { get; set; } = Color.Black;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

}