using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;

namespace Paper_io_server
{
    class ServerPeer : IPeer
    {
        User Player = null;
        Playertoken token = null;
        public GameServer Server { get; private set; }


        public ServerPeer(GameServer server, User player, Playertoken token)
        {
            this.Server = server;
            Player = player;
            Player.setPeer(this);
            player.setToken(token);
            this.token = token;
        }

       
        public void disconnected()
        {
            Request_exit();
        }

        private void Request_exit()
        {
            Server.Request_Exit(Player);
        }

        public void Receive(Packet packet)
        {
            switch ((PROTOCOL)packet.getprotocol())
            {
                case PROTOCOL.Player_ID:
                    //Player.setID(packet.pop_string());
                    //Server.Change_ID();
                    break;

                case PROTOCOL.Request_Change_Direction:
                    int player_no = packet.pop_int();
                    DIRECTION direction = (DIRECTION)packet.pop_object();

                    Server.Change_Direction(player_no, direction);
                    break;

                case PROTOCOL.Request_Change_ID:
                    int no = packet.pop_int();
                    string id = packet.pop_string();

                    Server.Change_ID(no, id);
                    break;
            }
        }

        public void send(Packet packet)
        {
            token.send(packet);
        }
            
    }
}
