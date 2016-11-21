using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AngularBlog.Interfaces;
using AngularBlog.Model;
using Microsoft.AspNetCore.Mvc;
using AngularBlog.Handlers;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularBlog.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        public IPostRepository _postRepository { get; set; }

        public PostController(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Post> Get(bool? isPublished)
        {
            List<Post> list;
            if (isPublished != null && isPublished.Value)
            {
                list = _postRepository.GetAll().Where(p => p.IsPublished == isPublished.Value).ToList();
            }
            else
            {
                list = _postRepository.GetAll().ToList();
            }
            return list;
        }

        [HttpGet("{id}", Name = "GetNoteById")]
        public IActionResult GetNoteById(Guid id)
        {
            var item = _postRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public void Post([FromBody]Post post)
        {
            if (post == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

           var folder = _postRepository.GetHostingEnviroment();
           PostHandler.SaveFilesToDisk(post, folder);
           _postRepository.Add(post);
        }

        [HttpPut("{id}")]
        public void Update(Guid id, [FromBody]Post post)
        {
            if (post == null || post.Id != id)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            var item = _postRepository.GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            var folder = _postRepository.GetFolder();
            PostHandler.SaveFilesToDisk(post, folder);
            _postRepository.Update(post);
        }

        [HttpPatch("{id}")]
        public void Update([FromBody] Post post, Guid id)
        {
            if (post == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            var item = _postRepository.GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            post.Id = item.Id;
            _postRepository.Update(item);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            _postRepository.Delete(post);
        }
    }
}
