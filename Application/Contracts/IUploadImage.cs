using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Contracts
{
    public interface IUploadImage<T> where T : class
    {
        string UpLoandImageFromFile(T entity);

    }
}
