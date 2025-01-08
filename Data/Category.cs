using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class Category
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public Category Parents { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public ICollection<PostCategory> PostCategorys { get; set; }

    }
}
