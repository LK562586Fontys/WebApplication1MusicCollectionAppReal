using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDataModel : IUserDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public DateTime JoinDate { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
