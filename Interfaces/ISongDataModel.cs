using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISongDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public DateTime DateReleased { get; set; }
        public IUserDataModel Artist { get; set; }
        public IPlaylistDataModel Album { get; set; }
    }
}
