using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SongDataModel
    {
        public int ID;
        public string name;
        public int weight;
        public DateTime dateReleased;
        public int artistID;
        public int albumID;
    }
}
