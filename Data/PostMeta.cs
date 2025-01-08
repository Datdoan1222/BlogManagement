using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class PostMeta
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public Post Posts { get; set; }
    }
}
