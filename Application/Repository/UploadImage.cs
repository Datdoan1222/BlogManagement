using BlogManagement.Contracts;
using BlogManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Repository
{
    public class UploadImage : IUploadImagePostCreateEditPost
    {

        public  string UpLoandImageFromFile(CreateEditPostVM entity)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
            string fileNameWithPath = Path.Combine(path, entity.File.FileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                entity.File.CopyTo(stream);

            }
            entity.ImagePath = "/Images/" + entity.File.FileName;
            return entity.ImagePath;
        }
    }
}
