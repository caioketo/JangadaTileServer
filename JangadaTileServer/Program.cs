using Alchemy;
using Alchemy.Classes;
using Proto;
using JangadaTileServer.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JangadaTileServer.Content;

namespace JangadaTileServer
{
    class Program
    {

        protected static ConcurrentDictionary<JWebClient, string> Clients = new ConcurrentDictionary<JWebClient, string>();

        static void Main(string[] args)
        {
            Game.GetInstance();
            var aServer = new WebSocketServer(81, IPAddress.Any)
            {
                OnReceive = OnReceive,
                OnSend = OnSend,
                OnConnected = OnConnect,
                OnDisconnect = OnDisconnect,
                TimeOut = new TimeSpan(0, 5, 0)
            };

            aServer.Start();

            // Accept commands on the console and keep it alive
            var command = string.Empty;
            while (command != "exit")
            {
                command = Console.ReadLine();
            }

            aServer.Stop();
        }


        public static void OnConnect(UserContext context)
        {
            Console.WriteLine("Client Connection From : " + context.ClientAddress);
            Clients.TryAdd(new JWebClient { Context = context }, String.Empty);
        }


        public static void OnReceive(UserContext context)
        {
            try
            {
                Messages messages = Messages.CreateBuilder().MergeFrom(
                    Google.ProtocolBuffers.ByteString.FromBase64(context.DataFrame.ToString())).Build();
                foreach (Networkmessage message in messages.NetworkmessageList)
                {
                    if (!Parser.Parse(message.Type, message, GetClient(context)))
                    {
                        //Disconnect
                    }
                }
            }
            catch (Exception e)
            {
            }
        }


        public static void OnSend(UserContext context)
        {
        }


        public static void OnDisconnect(UserContext context)
        {
            Console.WriteLine("Client Disconnected : " + context.ClientAddress);
            var user = Clients.Keys.Where(o => o.Context.ClientAddress == context.ClientAddress).Single();

            string trash; // Concurrent dictionaries make things weird

            Clients.TryRemove(user, out trash);
        }

        public static JWebClient GetClient(UserContext context)
        {
            return Clients.Keys.Where(o => o.Context.ClientAddress == context.ClientAddress).Single();
        }
    }
}
