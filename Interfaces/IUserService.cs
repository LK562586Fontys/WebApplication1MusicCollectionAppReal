using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserService
    {
        IUserDTO GetUserById(int id);
        Task<int?> VerifyLoginAndReturnUserId(string email, string password);
    }
}
