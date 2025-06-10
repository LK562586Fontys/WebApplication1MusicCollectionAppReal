using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepo;
        private readonly UserMapper mapper;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
            this.mapper = new UserMapper(userRepo);
        }

        public IUserDTO? GetUserById(int id)
        {
            var dto = userRepo.GetSpecificUser(id);
            return dto != null ? mapper.FromDataModel(dto) : null;
        }
    }
}
