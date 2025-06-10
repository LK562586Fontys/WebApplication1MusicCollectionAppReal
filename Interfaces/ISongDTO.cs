using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISongDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public DateTime DateReleased { get; set; }
        public IUserDTO Artist { get; set; }
        public IPlaylistDTO Album { get; set; }
    }
}
