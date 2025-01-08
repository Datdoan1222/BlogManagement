using BlogManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Models
{
    public class PostCommentVM
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public User Users { get; set; }
        public long? ParentId { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; }
        public string TimeCmt { get; set; }
        public int? TotalVotes { get; set; }
        public IEnumerable<PostComment> PostCommentsChild { get; set; }

    }
}
