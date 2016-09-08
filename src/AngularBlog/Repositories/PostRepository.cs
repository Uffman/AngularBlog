using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AngularBlog.Interfaces;
using AngularBlog.Model;
using Microsoft.AspNetCore.Hosting;

namespace AngularBlog.Repositories
{
    public class PostRepository : IPostRepository
    {
        private List<Post> postsList = new List<Post>();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string folder;

        public PostRepository(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            folder = Path.Combine(this._hostingEnvironment.ContentRootPath, "Data", "Posts");
            LoadData();
        }

        private void LoadData()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (string file in Directory.EnumerateFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                XElement document = XElement.Load(file);
                Post post = new Post()
                {
                    Id = new Guid(Path.GetFileNameWithoutExtension(file)),
                    Title = ReadValue(document, "title"),
                    Author = ReadValue(document, "author"),
                    Slug = ReadValue(document, "slug").ToLowerInvariant(),
                    Content = ReadValue(document, "content"),
                    CreatedDate = DateTime.Parse(ReadValue(document, "createddate")),
                    ModifiedDate = DateTime.Parse(ReadValue(document, "modifieddate")),
                    IsPublished = bool.Parse(ReadValue(document, "ispublished", "true"))
                };

                LoadComments(post, document);

                postsList.Add(post);
            }

            if (postsList.Any())
            {
                postsList.Sort((p1, p2) => p2.CreatedDate.CompareTo(p1.CreatedDate));
            }
        }

        private void LoadComments(Post post, XElement document)
        {
            var comments = document.Element("comments");
            if (comments == null)
            {
                return;
            }

            foreach (var node in comments.Elements("comment"))
            {
                Comment comment = new Comment()
                {
                    Id = new Guid(ReadAttribute(node, "id")),
                    Author = ReadValue(node, "author"),
                    Email = ReadValue(node, "email"),
                    Content = ReadValue(node, "content"),
                    CreatedDate = DateTime.Parse(ReadValue(node, "createddate")),
                    ModifiedDate = DateTime.Parse(ReadValue(node, "modifieddate"))
                };

                post.Comments.Add(comment);
            }
        }

        public IEnumerable<Post> GetAll()
        {
            return postsList;
        }

        public Post GetById(Guid id)
        {
            Post post = postsList.FirstOrDefault(p => p.Id == id);
            return post;
        }

        public Post GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Post Add(Post item)
        {
            string newFile = Path.Combine(folder, item.Id + ".xml");
            item.ModifiedDate = DateTime.UtcNow;

            XDocument newDocument = new XDocument(
                new XElement("post",
                new XElement("title", item.Title),
                new XElement("author", item.Author),
                new XElement("slug", item.Slug),
                new XElement("content", item.Content),
                new XElement("createddate", item.CreatedDate),
                new XElement("modifieddate", item.ModifiedDate),
                new XElement("comments", string.Empty),
                new XElement("ispublished", item.IsPublished)
                    )
                );

            XElement comments = newDocument.Element("comments");
            foreach (Comment comment in item.Comments)
            {
                comments.Add(
                    new XElement("comment",
                        new XElement("author", comment.Author),
                        new XElement("email", comment.Email),
                        new XElement("createddate", comment.CreatedDate.ToString("yyyy-MM-dd HH:m:ss")),
                        new XElement("modifieddate", comment.ModifiedDate.ToString("yyyy-MM-dd HH:m:ss")),
                        new XElement("content", comment.Content),
                        new XAttribute("id", comment.Id)
                    ));
            }

            if (!File.Exists(newFile))
            {
                postsList.Insert(0, item);
                postsList.Sort((p1, p2) => p2.CreatedDate.CompareTo(p1.CreatedDate));
            }

            SaveXml(newDocument);

            return item;
        }

        public bool Update(Post item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Post item)
        {
            postsList.Remove(item);
            string file = Path.Combine(folder, item.Id + ".xml");
            File.Delete(file);
            return true;
        }

        private static string ReadValue(XElement document, XName name, string defaultValue = "")
        {
            if (document.Element(name) != null)
            {
                return document.Element(name).Value;
            }

            return defaultValue;
        }

        private static string ReadAttribute(XElement element, XName name, string defaultValue = "")
        {
            if (element.Attribute(name) != null)
                return element.Attribute(name).Value;

            return defaultValue;
        }

        private void SaveXml(XDocument document)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tr = new StringWriter(sb);
            document.Save(tr);
        }
    }
}
