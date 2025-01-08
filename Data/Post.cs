using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class Post
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public User Users { get; set; }
        public long? ParentId { get; set; }
        public Post PostParent { get; set; }
        public string Title { get; set; }
        public string MetaTile { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public bool? Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool? HotPost { get; set; }
        public ICollection<PostCategory> Categories { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<PostMeta> PostMetas { get; set; }
        public ICollection<PostTag> PostTags { get; set; }




    }
}
