using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class PostCategory
    {
        public long PostId { get; set; }
        public Post Posts { get; set; }
        public long CategoryId { get; set; }
        public Category Categories{ get; set; }

    }
}
