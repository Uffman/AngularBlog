using System;
using AngularBlog.Model;

namespace AngularBlog.Interfaces
{
    public interface IPostRepository: IRepository<Post>
    {
        Post GetById(Guid id);
    }
}