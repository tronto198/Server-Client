using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using System.Net;
using System.Drawing;
using SharedLibaray;

namespace Paper_io_Client
{
    /// <summary>
    /// 9개에 0.24퍼
    /// 20 X 20 으로 시작
    /// 
    /// </summary>
    class MainProgram
    {
        private static MainProgram instance = null;
        SocketConnecter Connecter = null;
        int port = 0;
        IPAddress server_ip = null;
        Playertoken Server = null;
        static ClientPeer peer = null;
        Threading_Timer thread_cal = null;
        static string ID = "";

        public delegate void CalHandler();
        public static CalHandler callback_calculate = null;

        public delegate void mapHandler();
        public static mapHandler callback_DrawMap = null;

        public delegate void UserHandler();
        public static UserHandler callback_DrawUser = null;

        public delegate void LetterHandler();
        public static LetterHandler callback_DrawLetter = null;

        private MainProgram()
        {
            port = 5252;
            
            DoubleBuffering.callback_work += Draw;
            PacketManager.init(20);

            thread_cal = new Threading_Timer();
            thread_cal.setInterval(10);
            thread_cal.setCallback(new Action(delegate() {
                if(callback_calculate != null)
                    callback_calculate();
            }));
            thread_cal.Start();
        }

        public static MainProgram getinstance()
        {
            if(instance == null)
            {
                instance = new MainProgram();
            }
            return instance;
        }

        public static void get_this()
        {

        }


        public void Connect_start(string ipadrees, string id)
        {
            //127.0.0.1
            //121.183.230.146
            //server_ip = IPAddress.Parse("121.183.230.146");
            ID = id;
            server_ip = IPAddress.Parse(ipadrees);
            Connecter = new SocketConnecter(port);
            Connecter.Clientinit(server_ip);
            Connecter.callback_connected += Connected;
        }
        private void Connected(Playertoken token)
        {
            peer = new ClientPeer(token);
            Server = token;
            Connecter.Start_Receive(token);
        }
        public static void Send_id(int no)
        {
            
            Packet packet = Packet.create(PROTOCOL.Request_Change_ID);
            packet.push(no);
            packet.push(ID);
            peer.send(packet);

        }

        private void Draw()
        {
            DoubleBuffering.Instance().getGraphics.Clear(Color.LightBlue);
            if(callback_DrawMap != null)
            {
                callback_DrawMap();
            }
            if(callback_DrawUser != null)
            {
                callback_DrawUser();
            }
            if(callback_DrawLetter != null)
            {
                callback_DrawLetter();
            }
        }

        public void Close()
        {
            Connecter.Stop();
        }
    }


    class writing
    {
        float x, y;
        string str;
        Font thisfont = new Font("AR CENA", 10);
        Brush thisbrush;
        bool alive = false;

        public writing(int x, int y,string str , Color color)
        {
            thisbrush = new SolidBrush(color);
            this.x = x;
            this.y = y;
            this.str = str;
        }
        public writing(float x, float y,string str, Color color)
        {
            thisbrush = new SolidBrush(color);
            this.x = x;
            this.y = y;
            this.str = str;
        }

        public void setxy(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void setstr(string str)
        {
            this.str = str;
        }

        public void on()
        {
            if (!alive)
            {
                alive = true;
                MainProgram.callback_DrawLetter += Draw;
            }
        }

        public void off()
        {
            if (alive)
            {
                alive = false;
                MainProgram.callback_DrawLetter -= Draw;
            }
        }

        public void Draw()
        {
            DoubleBuffering.Instance().getGraphics.DrawString(str, thisfont, thisbrush, Map.getDrawPoint((int)x, (int)y - 15));
        }
    }
}
