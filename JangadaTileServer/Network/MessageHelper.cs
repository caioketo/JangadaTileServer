using Proto;
using JangadaTileServer.Content;
using JangadaTileServer.Content.World;
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

        public static void SendAreaDescription(JWebClient client, Area area)
        {
            AreaDescriptionPacket.Builder areaDesc = AreaDescriptionPacket.CreateBuilder();
            int minX = client.Player.Position.X - 19;
            int maxX = client.Player.Position.X + 19;
            int minY = client.Player.Position.Y - 13;
            int maxY = client.Player.Position.Y + 13;
            areaDesc.SetStartX(minX)
                .SetStartY(minY)
                .SetWidth(area.Terrain.Width)
                .SetHeight(area.Terrain.Height)
                .SetAreaId(area.Id);
            PlayerDescription playerDesc = PlayerDescription.CreateBuilder()
                .SetPlayerGuid(client.Player.CreatureGuid.ToString("N"))
                .SetPlayerPosition(Position.CreateBuilder()
                .SetX(client.Player.Position.X)
                .SetY(client.Player.Position.Y)
                .SetZ(client.Player.Position.Z)
                .Build()).Build();
            areaDesc.SetPlayer(playerDesc);
            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    Tile tile = area.Terrain.GetTile(x, y);
                    TileDescription.Builder tileDesc = TileDescription.CreateBuilder();
                    tileDesc.SetGroundId(tile.Ground.Id);
                    foreach (Item item in tile.Items)
                    {
                        tileDesc.AddItems(item.Id);
                    }
                    areaDesc.AddTiles(tileDesc.Build());
                }
            }



            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.AreaDescriptionPacket = areaDesc.Build();
            newMessage.Type = Networkmessage.Types.Type.AREA_DESCRIPTION;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }

        internal static void SendPlayerMove(JWebClient client, RequestMovementPacket.Types.MovementType movementType)
        {
            PlayerMovementPacket.Builder playerMovePacket = PlayerMovementPacket.CreateBuilder()
                .SetNewPosition(Position.CreateBuilder()
                .SetX(client.Player.Position.X)
                .SetY(client.Player.Position.Y)
                .SetZ(client.Player.Position.Z)
                .Build());
            int startX = client.Player.Position.X - 19;
            int endX = client.Player.Position.X + 19;
            int startY = client.Player.Position.Y - 13;
            int endY = client.Player.Position.Y + 13;
            switch (movementType)
            {
                case RequestMovementPacket.Types.MovementType.UP:
                    endY = startY;
                    startY--;
                    break;
                case RequestMovementPacket.Types.MovementType.DOWN:
                    startY = client.Player.Position.Y + 12;
                    endY = startY + 1;
                    break;
                case RequestMovementPacket.Types.MovementType.LEFT:
                    endX = startX;
                    startX--;
                    break;
                case RequestMovementPacket.Types.MovementType.RIGHT:
                    startX = client.Player.Position.X + 18;
                    endX = startX + 1;
                    break;
                default:
                    break;
            }
            if (startX < 0)
            {
                startX = 0;
            }
            if (startY < 0)
            {
                startY = 0;
            }
            MapSliceDescription.Builder mapSlice = MapSliceDescription.CreateBuilder()
                .SetStartX(startX)
                .SetEndX(endX)
                .SetStartY(startY)
                .SetEndY(endY);
            for (int x = startX; x < endX + 1; x++)
            {
                for (int y = startY; y < endY + 1; y++)
                {
                    Tile tile = client.Player.Area.Terrain.GetTile(x, y);
                    TileDescription.Builder tileDesc = TileDescription.CreateBuilder();
                    tileDesc.SetGroundId(tile.Ground.Id);
                    foreach (Item item in tile.Items)
                    {
                        tileDesc.AddItems(item.Id);
                    }
                    mapSlice.AddTiles(tileDesc.Build());
                }
            }

            Console.WriteLine("nX: " + client.Player.Position.X.ToString() + " - nY: " + client.Player.Position.Y.ToString());
            playerMovePacket.SetMapSlice(mapSlice.Build());
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.PlayerMovementPacket = playerMovePacket.Build();
            newMessage.Type = Networkmessage.Types.Type.PLAYER_MOVEMENT;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }
    }
}
