using JangadaTileServer.Content.Utils;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Scripting
{
    class ScriptManager
    {
        //TODO ADICIONAR FUNÇÕES PARA OS SCRIPTS!!
        private static ScriptManager instance;
        public static ScriptManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ScriptManager();
            }
            return instance;
        }

        public string Path()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        Lua state;

        public object[] ExecuteScript(LuaFunction script, params object[] args)
        {
            return script.Call(args);
        }

        public ScriptManager()
        {
            LoadScripts();
        }

        public void LoadScripts()
        {
            state = new Lua();
            state["scriptManager"] = this;
        }

        public void AddTask(int interval, LuaFunction function, params object[] args)
        {
            Console.WriteLine("AddTask");
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(interval);
                if (args == null)
                {
                    Console.WriteLine(function.Call().First());
                }
                else
                {
                    Console.WriteLine(function.Call(args).First());
                }
            });
        }

        public void LoadSkill(Skills skill)
        {
            state.DoFile(Path() + @"\Content\Scripting\Scripts\" + skill.ScriptName);
            skill.CastFunction = state["Cast"] as LuaFunction;
            var r = skill.CastFunction.Call(new Content.Creatures.Creature(1)).First();
        }
    }
}
