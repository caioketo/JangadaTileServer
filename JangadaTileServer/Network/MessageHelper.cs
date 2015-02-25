using Proto;
using JangadaTileServer.Content;
using JangadaTileServer.Content.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JangadaTileServer.Content.Creatures;

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

        public static AreaDescriptionPacket GenAreaDescription(JWebClient client)
        {
            Area area = client.Player.Area;
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
            areaDesc.SetPlayer(GenPlayerDesc(client.Player));
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

            foreach (Player player in area.PlayersInViewArea(client.Player.Position))
            {
                if (player.CreatureGuid != client.Player.CreatureGuid)
                {
                    areaDesc.AddPlayers(GenPlayerDesc(player));
                }
            }

            return areaDesc.Build();
        }

        internal static PlayerDescription GenPlayerDesc(Player player)
        {
            return PlayerDescription.CreateBuilder()
                .SetPlayerGuid(player.CreatureGuid.ToString("N"))
                .SetName(player.Name)
                .SetSpeed(player.Speed)
                .SetPlayerPosition(Position.CreateBuilder()
                .SetX(player.Position.X)
                .SetY(player.Position.Y)
                .SetZ(player.Position.Z)
                .Build()).Build();
        }

        internal static MapSliceDescription GenSliceDesc(SliceInfo slice)
        {
            MapSliceDescription.Builder mapSlice = MapSliceDescription.CreateBuilder()
                .SetStartX(slice.startX)
                .SetEndX(slice.endX)
                .SetStartY(slice.startY)
                .SetEndY(slice.endY);
            for (int x = slice.startX; x < slice.endX + 1; x++)
            {
                for (int y = slice.startY; y < slice.endY + 1; y++)
                {
                    Tile tile = slice.area.Terrain.GetTile(x, y);
                    TileDescription.Builder tileDesc = TileDescription.CreateBuilder();
                    tileDesc.SetGroundId(tile.Ground.Id);
                    foreach (Item item in tile.Items)
                    {
                        tileDesc.AddItems(item.Id);
                    }
                    mapSlice.AddTiles(tileDesc.Build());
                }
            }

            return mapSlice.Build();
        }

        internal class SliceInfo
        {
            public int startX { get; set; }
            public int endX { get; set; }
            public int startY { get; set; }
            public int endY { get; set; }
            public Area area { get; set; }
        }

        internal static SliceInfo GenSliceInfo(Player player, RequestMovementPacket.Types.MovementType type)
        {
            SliceInfo slice = new SliceInfo();
            slice.area = player.Area;
            slice.startX = player.Position.X - 19;
            slice.endX = player.Position.X + 19;
            slice.startY = player.Position.Y - 13;
            slice.endY = player.Position.Y + 13;
            switch (type)
            {
                case RequestMovementPacket.Types.MovementType.UP:
                    slice.endY = slice.startY;
                    slice.startY--;
                    break;
                case RequestMovementPacket.Types.MovementType.DOWN:
                    slice.startY = player.Position.Y + 12;
                    slice.endY = slice.startY + 1;
                    break;
                case RequestMovementPacket.Types.MovementType.LEFT:
                    slice.endX = slice.startX;
                    slice.startX--;
                    break;
                case RequestMovementPacket.Types.MovementType.RIGHT:
                    slice.startX = player.Position.X + 18;
                    slice.endX = slice.startX + 1;
                    break;
                default:
                    break;
            }
            if (slice.startX < 0)
            {
                slice.startX = 0;
            }
            if (slice.startY < 0)
            {
                slice.startY = 0;
            }
            return slice;
        }

        internal static void SendPlayerMove(JWebClient client)
        {
            PlayerMovementPacket.Builder playerMovePacket = PlayerMovementPacket.CreateBuilder()
                .SetNewPosition(Position.CreateBuilder()
                .SetX(client.Player.Position.X)
                .SetY(client.Player.Position.Y)
                .SetZ(client.Player.Position.Z)
                .Build());


            playerMovePacket.SetMapSlice(GenSliceDesc(GenSliceInfo(client.Player, client.Player.LastMoveType)));
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.PlayerMovementPacket = playerMovePacket.Build();
            newMessage.Type = Networkmessage.Types.Type.PLAYER_MOVEMENT;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }

        internal static void SendPlayerLogin(JWebClient client, Player player)
        {
            PlayerLoginPacket playerLogin = PlayerLoginPacket.CreateBuilder()
                .SetPlayer(GenPlayerDesc(player)).Build();

            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.PlayerLoginPacket = playerLogin;
            newMessage.Type = Networkmessage.Types.Type.PLAYER_LOGIN;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }


        internal static void SendCreatureMove(JWebClient client, Player player)
        {
            CharacterMovementPacket charMovement = CharacterMovementPacket.CreateBuilder()
                .SetPlayer(GenPlayerDesc(player)).Build();

            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.CharacterMovementPacket = charMovement;
            newMessage.Type = Networkmessage.Types.Type.CHARACTER_MOVEMENT;;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }

        internal static void SendInitialPacket(JWebClient client)
        {
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.AreaDescriptionPacket = GenAreaDescription(client);
            newMessage.Type = Networkmessage.Types.Type.AREA_DESCRIPTION;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            Send(messagesToSend, client);
        }
    }
}
