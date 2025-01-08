using BlogManagement.Contracts;
using BlogManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogManagementDBContext _context;
        private IGenericReponsitory<Post> _posts;
        private IGenericReponsitory<User> _users;
        private IGenericReponsitory<PostComment> _postComments;
        private IGenericReponsitory<PostCategory> _postCategories;
        private IGenericReponsitory<Category> _categories;
        private IGenericReponsitory<PostMeta> _postMetas;
        private IGenericReponsitory<PostTag> _postTags;
        private IGenericReponsitory<Tag> _tags;
        public UnitOfWork(BlogManagementDBContext context)
        {
            _context = context;
        }

        public IGenericReponsitory<Post> Posts
            => _posts ??= new GenericRepository<Post>(_context);

        public IGenericReponsitory<User> Users
            => _users ??= new GenericRepository<User>(_context);
        public IGenericReponsitory<PostComment> PostComments
            => _postComments ??= new GenericRepository<PostComment>(_context);
        public IGenericReponsitory<PostCategory> PostCategories
            => _postCategories ??= new GenericRepository<PostCategory>(_context);
        public IGenericReponsitory<Category> Categories
            => _categories ??= new GenericRepository<Category>(_context);

        public IGenericReponsitory<PostMeta> PostMetas
            => _postMetas ??= new GenericRepository<PostMeta>(_context);

        public IGenericReponsitory<PostTag> PostTags
            => _postTags ??= new GenericRepository<PostTag>(_context);

        public IGenericReponsitory<Tag> Tags
            => _tags ??= new GenericRepository<Tag>(_context);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
