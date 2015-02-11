using Jangada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Network
{
    class MessageHelper
    {

        public static void Send(Messages message, JWebClient client)
        {
            try
            {
                client.Context.Send(message.ToByteString().ToBase64());
            }
            catch (Exception)
            {
                //Game.GetInstance().PlayerLogout(connection.player);
            }
        }


        public static void SendCharacterList(JWebClient client, List<Character> characters)
        {
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.CharactersPacket = CharactersPacket.CreateBuilder().AddRangeCharacterList(characters).Build();
            newMessage.Type = Networkmessage.Types.Type.CHARACTERS;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }
    }
}
