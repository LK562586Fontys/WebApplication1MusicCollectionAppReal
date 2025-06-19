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


		public UserMapper()
		{
		}
		public User FromDataModel(IUserDataModel dataModel)
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
		public User FromDataModel(User user)
		{
			return user;
		}
	}
}
