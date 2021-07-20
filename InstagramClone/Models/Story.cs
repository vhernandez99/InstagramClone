using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        //public int IsActive { get; set; }

        public static IEnumerable<Story> GetAllStories()
        {
            return new List<Story>
            {
                new Story{Id=1,ImageUrl="face1.jpg"},
                new Story{Id=2,ImageUrl="face2.jpg"},
                new Story{Id=3,ImageUrl="face1.jpg"},
                new Story{Id=4,ImageUrl="face1.jpg"},
                new Story{Id=5,ImageUrl="face2.jpg"},
                new Story{Id=6,ImageUrl="face1.jpg"},
                new Story{Id=7,ImageUrl="face2.jpg"},
                new Story{Id=8,ImageUrl="face1.jpg"},
                new Story{Id=9,ImageUrl="face2.jpg"},
                new Story{Id=10,ImageUrl="face1.jpg"},
                new Story{Id=11,ImageUrl="face2.jpg"},
                new Story{Id=12,ImageUrl="face1.jpg"},
                new Story{Id=13,ImageUrl="face2.jpg"}
            };
        }
    }
}
