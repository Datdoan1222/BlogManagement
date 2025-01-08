using BlogManagement.Data;
using BlogManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Contracts
{
    public interface IUserRepository
    {

        string GetPasswordHash(string password);

    }
}
