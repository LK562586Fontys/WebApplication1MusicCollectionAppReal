using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public static class UserMapper
    {
        public static User FromDataModel(UserDataModel dataModel)
        {
            return new User
            {
                ID = dataModel.ID,
                Name = dataModel.userName
                // Add other fields if your logic model has more
            };
        }
    }
}
