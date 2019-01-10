using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Network;

namespace Paper_io_server
{
    class MasterServer
    {
        ///SocketListener를 통해서 새로운 접속 관리
        ///새로운 클라이언트가 접속하면 게임서버로 연결시킴
        ///
        public int PORT {get; private set;}
        private static MasterServer instance = null;

        object server_locker = new object();
        private List<GameServer> ServerList = null;
        private SocketConnecter socketConnecter = null;

        private RandomPool ServerCodePool = null;

        private MasterServer()
        {
            ServerCodePool = new RandomPool(1000, 9999);
            PORT = 5252;
            PacketManager.init(1000);
            ServerList = new List<GameServer>();
            socketConnecter = new SocketConnecter(PORT);
            socketConnecter.callback_connected += newClient;
            socketConnecter.Serverinit();

            Server_generate();
        }

        public static MasterServer getinstance()
        {
            if(instance == null)
            {
                instance = new MasterServer();
            }
            return instance;
        }

        public List<int> getServerlist()
        {
            List<int> list = new List<int>();
            lock (server_locker)
            {
                foreach(GameServer g in ServerList)
                {
                    list.Add(g.Servercode);
                }
            }
            return list;
        }
        public GameServer getServer(int Servercode)
        {
            var selectedServer = from server in ServerList
                                              where server.Servercode == Servercode
                                              select server;

            if(selectedServer.Count() == 1)
            {
                return selectedServer.First();
            }
            else
            {
                Paper_io_Server.RecordLog("같은 코드 2개 이상! : " + Servercode);
            }
            return null;
        }

        private void newClient(Playertoken token)
        {
            try
            {
                GameServer Server;

                //게임 서버로 전달
                IEnumerable<GameServer> selectedServer = from server in ServerList
                                             where server.getFull() == false
                                             select server;


                if (selectedServer.Count() != 0)
                {
                    Server = selectedServer.First();
                }
                else
                {
                    Server = Server_generate();
                    
                }
                Server.newPlayer(token);

                socketConnecter.Start_Receive(token);
            }
            catch(Exception e)
            {
                Paper_io_Server.RecordLog(e.ToString());
            }
        }
        

        private GameServer Server_generate()
        {
            GameServer new_server = new GameServer(ServerCodePool.pop());
            ServerList.Add(new_server);
            return new_server;
        }

        public void Stop()
        {
            socketConnecter.Stop();
        }
    }
}
