using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Utils
{
    class Skills
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TextureId { get; set; }
        public float CoolDown { get; set; }
        public float Distance { get; set; }
        public bool AutoCast { get; set; }
        public LuaFunction Cast { get; set; }
    }
}
