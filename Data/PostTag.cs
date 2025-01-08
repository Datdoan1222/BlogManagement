using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.Data
{
    public class PostTag
    {
        public long PostId { get; set; }
        public Post Posts { get; set; }
        public long TagId { get; set; }
        public Tag Tags { get; set; }

    }
}
