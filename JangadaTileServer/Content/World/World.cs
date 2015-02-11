using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class World
    {
        public List<Area> Areas { get; set; }

        public World()
        {
            this.Areas = new List<Area>();
        }
    }
}
