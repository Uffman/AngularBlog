using System;
using System.Collections.Generic;

namespace AngularBlog.Model
{
    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid();
            Title = "My new post";
            Author = "UserName";
            Content = "the content";
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            Comments = new List<Comment>();
            IsPublished = true;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
