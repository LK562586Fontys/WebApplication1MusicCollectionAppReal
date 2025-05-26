using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PlaylistDataModel
    {
        public int ID;
        public string Name;
        public DateTime DateAdded;
        public byte[]? Photo;
        public int? Creator;
    }
}
