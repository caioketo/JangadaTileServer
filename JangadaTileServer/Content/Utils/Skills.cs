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
        public string ScriptName { get; set; }
        public Lua CastState { get; set; }
        public LuaFunction CastFunction { get; set; }

        public Skills(int id)
        {
            this.Id = id;
            this.ScriptName = "teste" + id.ToString() + ".lua";
            Scripting.ScriptManager.GetInstance().LoadSkill(this);
        }
    }
}
