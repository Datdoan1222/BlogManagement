using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class PostComment
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public Post Posts { get; set; }
        public long UserId { get; set; }
        public User Users { get; set; }
        public long? ParentId { get; set; }
        public PostComment Parents { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Content { get; set; }
        public int? TotalVotes { get; set; }
    }
}
