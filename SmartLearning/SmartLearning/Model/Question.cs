using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SmartLearning
{
   public class Question
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string title { get; set; }
        public string question { get; set; }
        public bool isActive { get; set; }
        public List<Option> options { get; set; }
        public string type { get; set; }

        public Question()
        {
            options = new List<Option>();
        }
    }

    public class Option
    {
        public string option { get; set; }
        public string status { get; set; }

    }
}

