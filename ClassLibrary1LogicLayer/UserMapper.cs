using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserMapper
    {
        private readonly IUserRepository userRepository;

        public UserMapper(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public User FromDataModel(IUserDTO dataModel)
        {
			return new User(
	        dataModel.ID,
	        dataModel.Name,
	        dataModel.EmailAddress,
	        dataModel.PasswordHash,
	        dataModel.ProfilePhoto,
	        dataModel.JoinDate
            );
		}
    }
}
