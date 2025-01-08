using BlogManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Contracts
{
    interface IUploadImagePostCreateEditPost : IUploadImage<CreateEditPostVM>
    {
    }
}
