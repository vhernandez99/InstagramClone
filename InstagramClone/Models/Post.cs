﻿using System.Collections.Generic;

namespace InstagramClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public static IEnumerable<Post> GetAllPosts()
        {
            return new List<Post>
        {
            new Post { Id = 1, UserName = "User1", ImageUrl = "post1.jpg" },
            new Post { Id = 2, UserName = "User2", ImageUrl = "post2.jpg" },
            new Post { Id = 3, UserName = "User3", ImageUrl = "post1.jpg" },
            new Post { Id = 4, UserName = "User4", ImageUrl = "post2.jpg" },
            new Post { Id = 5, UserName = "User5", ImageUrl = "post1.jpg" },
            new Post { Id = 6, UserName = "User6", ImageUrl = "post2.jpg" },
            new Post { Id = 7, UserName = "User7", ImageUrl = "post1.jpg" },
            new Post { Id = 8, UserName = "User8", ImageUrl = "post2.jpg" },
            new Post { Id = 9, UserName = "User9", ImageUrl = "post1.jpg" },
            new Post { Id = 10, UserName = "User10", ImageUrl = "post2.jpg" },
            new Post { Id = 11, UserName = "User11", ImageUrl = "post1.jpg" }
        };
        }
    }


}
