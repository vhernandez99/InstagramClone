using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        //relation
        public string UserName { get; set; }
        //public int IsActive { get; set; }

        public static IEnumerable<Story> GetAllStories()
        {
            return new List<Story>
            {
                new Story{Id=1,UserName="User1",ImageUrl="face1.jpg"},
                new Story{Id=2,UserName="User2",ImageUrl="face2.jpg"},
                new Story{Id=3,UserName="User3",ImageUrl="face1.jpg"},
                new Story{Id=4,UserName="User4",ImageUrl="face1.jpg"},
                new Story{Id=5,UserName="User5",ImageUrl="face2.jpg"},
                new Story{Id=6,UserName="User6",ImageUrl="face1.jpg"},
                new Story{Id=7,UserName="User7",ImageUrl="face2.jpg"},
                new Story{Id=8,UserName="User8",ImageUrl="face1.jpg"},
                new Story{Id=9,UserName="User9",ImageUrl="face2.jpg"},
                new Story{Id=10,UserName="User10",ImageUrl="face1.jpg"},
                new Story{Id=11,UserName="User11",ImageUrl="face2.jpg"},
                new Story{Id=12,UserName="User12",ImageUrl="face1.jpg"},
                new Story{Id=13,UserName="User13",ImageUrl="face2.jpg"}
            };
        }
    }
}
