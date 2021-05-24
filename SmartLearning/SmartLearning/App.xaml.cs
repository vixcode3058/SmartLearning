using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SmartLearning
{
    public partial class App : Application
    {
       public static List<TopicModel> _topicList;
        public static List<Question> _questionList;
        public static string _fileName = "Topics.xml";
        public static string _questionFile = "Questions.xml";
        static MyDatabase database;

        public static MyDatabase Database
        {
            get
            {

                if (database == null)
                {
                    database = new MyDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("Result.db3"));
                }
                return database;
            }
        }


        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
