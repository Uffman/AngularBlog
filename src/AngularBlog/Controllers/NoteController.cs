using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngularBlog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AngularBlog.Model;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularBlog.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        public INotesRepository _repository { get; set; }

        public NoteController(INotesRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Note> Get()
        {
            var list = _repository.GetAll();
            return list;
        }

        [HttpGet("{id}", Name = "GetNote")]
        public IActionResult GetById(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public void Post([FromBody]Note note)
        {
            if (note == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            _repository.Add(note);
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromBody]Note note)
        {
            if (note == null || note.Id != id)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            var item = _repository.GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            _repository.Update(note);
        }

        [HttpPatch("{id}")]
        public void Update([FromBody] Note note, int id)
        {
            if (note == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            var item = _repository.GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            note.Id = item.Id;
            _repository.Update(item);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var todo = _repository.GetById(id);
            if (todo == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            _repository.Delete(todo);
        }
    }
}
