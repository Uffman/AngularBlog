using System;
using System.Collections.Generic;
using System.IO;
using AngularBlog.Interfaces;
using AngularBlog.Model;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AngularBlog.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private List<Note> notesList = new List<Note>();
        private XDocument document;
        private IHostingEnvironment _hostingEnvironment;
        private readonly string folder;
        private string filePath;

        public NotesRepository(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            folder = Path.Combine(this._hostingEnvironment.ContentRootPath, "Data", "Notes");
            filePath = Path.Combine(folder, "data.xml");
            if (File.Exists(filePath))
            {
                document = XDocument.Load(filePath);
                foreach (XElement node in document.Descendants("note"))
                {
                    Note note = new Note
                    {
                        Id = Int32.Parse(node.Descendants("id").FirstOrDefault().Value),
                        To = node.Descendants("to").FirstOrDefault().Value,
                        From = node.Descendants("from").FirstOrDefault().Value,
                        Heading = node.Descendants("heading").FirstOrDefault().Value,
                        Body = node.Descendants("body").FirstOrDefault().Value
                    };

                    notesList.Add(note);
                }
            }
        }

        public IEnumerable<Note> GetAll()
        {
            return notesList;
        }

        public Note GetById(int id)
        {
            return notesList.Find(p => p.Id == id);
        }

        public Note Add(Note item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = notesList.Count + 1;

            XElement newNode = new XElement("note");
            XElement id = new XElement("id") { Value = item.Id.ToString() };
            XElement to = new XElement("to") { Value = item.To };
            XElement from = new XElement("from") { Value = item.From };
            XElement heading = new XElement("heading") { Value = item.Heading };
            XElement body = new XElement("body") { Value = item.Body };
            newNode.Add(id, to, from, heading, body);
            document.Root.Add(newNode);

            SaveXml();

            return item;
        }

        public bool Update(Note item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            XElement note = document.Descendants("note").FirstOrDefault(n => Int32.Parse(n.Descendants("id").FirstOrDefault().Value) == item.Id);
            note.SetElementValue("to", item.To);
            note.SetElementValue("from", item.From);
            note.SetElementValue("heading", item.Heading);
            note.SetElementValue("body", item.Body);

            SaveXml();

            return true;
        }

        public bool DeleteById(int id)
        {
            document.Root.Descendants("note").Where(n => Int32.Parse(n.Descendants("id").First().Value) == id).Remove();
            SaveXml();

            return true;
        }

        public bool Delete(Note item)
        {
            throw new NotImplementedException();
        }

        private void SaveXml()
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tr = new StringWriter(sb);
            document.Save(tr);
        }
    }
}
