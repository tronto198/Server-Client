using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using SharedLibaray;
using System.Diagnostics;

namespace Paper_io_server
{
    class GameServer
    {
        public int Servercode { get; private set; }
        Stopwatch ServerTime = null;

        public User[] Userarr = new User[7];
        private int Max = 6;
        private int Playerno = 0;
        RandomPool Playerno_Pool;
        RandomPool colorpool = new RandomPool(1, 5);
        RandomPool xyPool = new RandomPool(2, 200);
        Threading_Timer calculate = new Threading_Timer();

        public GameServer(int Servercode)
        {
            this.Servercode = Servercode;
            ServerTime = new Stopwatch();
            Playerno_Pool = new RandomPool(1, 5);
            calculate.setCallback(cal);
            calculate.setInterval(10);
            calculate.Start();
        }

        public void newPlayer(Playertoken token)
        {
            Playerno++;
            User player = new User();
            token.setIPeer(new ServerPeer(this, player, token));
            player.setPlayer_no(Playerno_Pool.pop());


            Record(player, "Connected");


            //보냄
            Packet packet = Packet.create(PROTOCOL.Player_no);
            packet.push(player.Player_no);
            player.Peer.send(packet);


            Packet packets = Packet.create(PROTOCOL.Note_Servertime);
            packets.push(ServerTime.ElapsedMilliseconds);
            player.Peer.send(packets);

            player.x = xyPool.pop();
            player.y = xyPool.pop();
            COLOR color = (COLOR)colorpool.pop();
            player.setCOLOR(color);
            player.setDirection(DIRECTION.Up);


            for (int i = 0; i < 7; i++)
            {

                if (Userarr[i] != null)
                {
                    Packet olduserinfo = Packet.create(PROTOCOL.Player);
                    olduserinfo.push(ServerTime.ElapsedMilliseconds);
                    olduserinfo.push(Userarr[i]);
                    player.Peer.send(olduserinfo);
                }
            }
           
            Packet tome = Packet.create(PROTOCOL.Player);
            tome.push(ServerTime.ElapsedMilliseconds);
            tome.push(player);
            

            Packet newuserinfo = Packet.create(PROTOCOL.Player);
            newuserinfo.push(ServerTime.ElapsedMilliseconds);
            newuserinfo.push(player);

            Send_all(newuserinfo);

            Userarr[player.Player_no] = player;
            player.Peer.send(tome);
        }

        public bool getFull()
        {
            if (Playerno < Max)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void Request_Exit(User player)
        {
            Playerno--;
            Userarr[player.Player_no] = null;
            //max 계산

            Playerno_Pool.push(player.Player_no);
            colorpool.push((int)player.color);
            //플레이어 아웃 send


            Packet outevent = Packet.create(PROTOCOL.Player_exit);
            outevent.push(player);

            Send_all(outevent);
            Record(player, "Exit");
        }

        private void Record(User player, String str)
        {
            Paper_io_Server.RecordLog("Servercode : " + Servercode + " . " + player.ID + " " + player.Player_no + " " + str);
        }


        public void cal()
        {
            for (int i = 0; i < 7; i++)
            {
                if(Userarr[i] != null)
                {
                    Userarr[i].Move();
                }
            }

        }

        public void Change_Direction(int player_no, DIRECTION direction)
        {
            Userarr[player_no].setDirection(direction);
            Packet packet = Packet.create(PROTOCOL.Accept_Change_Direction);
            packet.push(player_no);
            packet.push(direction);

            Send_all(packet);
        }

        public void Change_ID(int player_no, string id)
        {
            Userarr[player_no].setID(id);

            Userarr[player_no].setID(id);
            Packet pakcet = Packet.create(PROTOCOL.Accept_Chage_ID);
            pakcet.push(player_no);
            pakcet.push(id);

            Send_all(pakcet);

            Record(Userarr[player_no], " id 변경");
        }


        public void Send_all(Packet packet)
        {
            for(int i = 0; i < 7; i++)
            {
                if(Userarr[i] != null)
                {
                    Packet copyed = (Packet)packet.Clone();

                    Userarr[i].Peer.send(copyed);
                }
                
            }
            packet.delete();
        }


        
    }
}
