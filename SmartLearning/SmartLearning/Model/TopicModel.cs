using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartLearning
{
  public  class TopicModel
    {

        [PrimaryKey]
        public int Id { get; set; }

        public String Title { get; set; }
        public String Description { get; set; }
        public String ImageName { get; set; }
        public String Example { get; set; }
        public String Learned { get; set; }

        }

}


