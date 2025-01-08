using BlogManagement.Data;
using System;
using System.Threading.Tasks;

namespace BlogManagement.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericReponsitory<Post> Posts { get;  }
        IGenericReponsitory<User> Users { get;  }
        IGenericReponsitory<PostComment> PostComments { get;  }
        IGenericReponsitory<PostCategory> PostCategories { get;  }
        IGenericReponsitory<Category> Categories { get;  }
        IGenericReponsitory<PostMeta> PostMetas { get;  }
        IGenericReponsitory<PostTag> PostTags { get;  }
        IGenericReponsitory<Tag> Tags { get;  }
        Task Save();

    }
}
