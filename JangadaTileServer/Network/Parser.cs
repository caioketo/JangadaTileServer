﻿using Jangada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Network
{
    class Parser
    {
        public static bool Parse(Networkmessage.Types.Type type, Networkmessage message, JWebClient client)
        {
            switch (type)
            {
                case Networkmessage.Types.Type.LOGIN:
                    Console.WriteLine("Login: " + message.LoginPacket.Login);
                    Console.WriteLine("Password: " + message.LoginPacket.Password);
                    List<Character> chars = new List<Character>();
                    chars.Add(Character.CreateBuilder()
                        .SetId(1)
                        .SetName("Keto")
                        .SetInfo("Level: 1")
                        .Build());
                    chars.Add(Character.CreateBuilder()
                        .SetId(1)
                        .SetName("Keto2")
                        .SetInfo("Level: 100")
                        .Build());
                    MessageHelper.SendCharacterList(client, chars);
                    break;
                case Networkmessage.Types.Type.SELECTEDCHAR:
                    //Game.GetInstance().OnPlayerLogin(connection);
                    Console.WriteLine("Selected char id = " + message.SelectCharacterPacket.Id.ToString());
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}