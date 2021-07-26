using System;
using System.Collections.Generic;
using System.IO;

namespace InstagramClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PostImageUrl { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string FullUserImageUrl => AppSettings.ApiUrl + UserImageUrl;
        public string FullPostImageUrl => AppSettings.ApiUrl + PostImageUrl;
        public DateTime PostedTime { get; set; }

        //public string ImageUrl { get; set; }
        //public string ImageUrl2 { get; set; }
        //public string ImageUrl3 { get; set; }
        //public static IEnumerable<Post> GetAllPosts()
        //{
        //    return new List<Post>
        //{
        //    new Post { Id = 1, UserName = "User1", ImageUrl = "post1.jpg",ImageUrl2="post1.jpg", ImageUrl3 = "post2.jpg" },
        //    new Post { Id = 2, UserName = "User2", ImageUrl = "post1.jpg",ImageUrl2="post1.jpg", ImageUrl3 = "post2.jpg" },
        //    new Post { Id = 3, UserName = "User3", ImageUrl = "post1.jpg",ImageUrl2="post1.jpg", ImageUrl3 = "post2.jpg" },
        //    //new Post { Id = 4, UserName = "User4", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 5, UserName = "User5", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 6, UserName = "User6", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 7, UserName = "User7", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 8, UserName = "User8", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg"},
        //    //new Post { Id = 9, UserName = "User9", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 10, UserName = "User10", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" },
        //    //new Post { Id = 11, UserName = "User11", ImageUrl = "post1.jpg",ImageUrl2="post2.jpg" }
        //};
        //}

    }


}
