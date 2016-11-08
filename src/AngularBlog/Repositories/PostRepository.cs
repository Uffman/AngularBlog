using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AngularBlog.Interfaces;
using AngularBlog.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Text;

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
            if (postsList.Any())
            {
                postsList = new List<Post>();
            }

            foreach (string file in Directory.EnumerateFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                XElement document = XElement.Load(file, LoadOptions.None);
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

            XDocument newDocument = new XDocument(new XDeclaration("1.0", "utf-8", null),
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

            SaveXml(newDocument, newFile);

            return item;
        }

        public bool Update(Post item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            string filePath = Path.Combine(folder, item.Id + ".xml"); 
            TextReader reader = new StringReader(File.ReadAllText(filePath));
            XDocument xDocument = XDocument.Load(reader);
            XElement post = xDocument.Descendants("post").FirstOrDefault();
            
            post.Descendants("title").FirstOrDefault().Value = item.Title;
            post.Descendants("slug").FirstOrDefault().Value = item.Slug;
            post.Descendants("content").FirstOrDefault().Value = item.Content;
            post.Descendants("modifieddate").FirstOrDefault().Value = DateTime.UtcNow.ToString();
            post.Descendants("ispublished").FirstOrDefault().Value = item.IsPublished.ToString();

            SaveXml(xDocument, filePath);

            LoadData();
            return true;
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

        private void SaveXml(XDocument document, string filePath)
        {
            document.Declaration = new XDeclaration("1.0", "utf-8", null);
            StringWriter writer = new Utf8StringWriter();
            document.Save(writer, SaveOptions.None);
            File.WriteAllText(filePath, writer.ToString());
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
