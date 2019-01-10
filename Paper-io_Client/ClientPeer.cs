using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using SharedLibaray;
using System.Windows.Forms;
using System.Diagnostics;

namespace Paper_io_Client
{
    class ClientPeer : IPeer
    {
        Playertoken token = null;
        Map map = new Map();

        public ClientPeer(Playertoken token)
        {
            this.token = token;
            token.setIPeer(this);
            Key_input.Key_in += keyin;
        }

        public void disconnected()
        {
            System.Windows.Forms.MessageBox.Show("disconnected");
            Timer_State timer = Timer_State.getinstance();
            timer.Stop();
        }

        public void Receive(Packet packet)
        {

            switch ((PROTOCOL)packet.protocol)
            {
                case PROTOCOL.Player_no:
                    int no = packet.pop_int();
                    map.setnum(no);

                    MainProgram.Send_id(no);
                    break;

                case PROTOCOL.Note_Servertime:
                    long time = packet.pop_long();
                    map.settime(time);
                    break;

                case PROTOCOL.Player:
                    long timems = packet.pop_long();
                    User user = packet.pop_object() as User;
                    
                    user.setPeer(this);
                    user.setToken(token);
                    map.addplayer(user, timems);

                    break;

                case PROTOCOL.Player_exit:
                    User exit_user = packet.pop_object() as User;
                    map.User_exit(exit_user);
                    break;


                case PROTOCOL.Accept_Change_Direction:
                    int player_no = packet.pop_int();
                    DIRECTION direction = (DIRECTION)packet.pop_object();
                    map.Change_Direction(player_no, direction);
                    break;

                case PROTOCOL.Accept_Chage_ID:
                    int no2 = packet.pop_int();
                    string str = packet.pop_string();
                    map.Change_ID(no2, str);
                    break;
            }
            
        }

        public void send(Packet packet)
        {
            token.send(packet);
        }

        public void keyin(Keys key)
        {
            DIRECTION direction = 0;

            switch(key)
            {
                case Keys.Left:
                case Keys.A:
                    direction = DIRECTION.Left;
                    break;

                case Keys.Right:
                case Keys.D:
                    direction = DIRECTION.Right;
                    break;

                case Keys.Up:
                case Keys.W:
                    direction = DIRECTION.Up;
                    break;

                case Keys.Down:
                case Keys.S:
                    direction = DIRECTION.Down;
                    break;

                default:

                    return;
            }
            Packet packet = map.Request_Change_Direction(direction);
            send(packet);
        }
        


        
    }
    class Map
    {
        object player_locker = new object();
        User[] Players = new User[7];
        int[,] map = new int[50, 75];
        public static int si = 35;    //시야

        static Point screen = new Point();
        public static int dot = 20; //한칸 도트
        public int my_no = 0;
        writing[] user_writing = new writing[7];
        List<writing> letterlist = new List<writing>();
        Stopwatch time;
        long Start_Servertime;

        public Map()
        {
            for (int i = 0; i < 7; i++)
            {
                Players[i] = null;
            }

            MainProgram.callback_calculate += cal;
            MainProgram.callback_calculate += cal_screen;
            MainProgram.callback_DrawMap += DrawMap;
            MainProgram.callback_DrawUser += DrawPlayer;
        }
        public void setnum(int no)
        {
            my_no = no;
        }

        public void settime(long no)
        {
            time = new Stopwatch();
            Start_Servertime = no;
        }

      

        public Packet Request_Change_Direction(DIRECTION direction)
        {

            Packet packet = Packet.create(PROTOCOL.Request_Change_Direction);
            packet.push(my_no);
            packet.push(direction);

            return packet;
        }

        public void Change_Direction(int player_no, DIRECTION direction)
        {
            Players[player_no].setDirection(direction);
        }

        public void Change_ID(int player_no, string id)
        {
            Players[player_no].setID(id);
            user_writing[player_no].setstr(id);
        }


        public void addplayer(User player, long timems)
        {
            lock (player_locker)
            {
                player.setCOLOR(player.color);
                //lock

                Players[player.Player_no] = player;

                long delay = time.ElapsedMilliseconds - (timems - Start_Servertime);
                player.sync(delay);
            }

            writing id = new writing(player.x, player.y, player.ID, Color.Black);
            user_writing[player.Player_no] = id;
            player.callback_move += id.setxy;
            id.on();
        }

        public void User_exit(User player)
        {
            lock (player_locker)
            {
                Players[player.Player_no] = null;
            }
            user_writing[player.Player_no].off();
            user_writing[player.Player_no] = null;
        }

        public void DrawMap()
        {
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 75; j++)
                {
                    int p = map[i, j] / 10;
                    if (Players[p] != null)
                    {
                        DoubleBuffering.Instance().getGraphics.FillRectangle(Players[p].brush, new Rectangle(j * dot - screen.X, i * dot - screen.Y, dot, dot));
                    }


                }
            }


        }

        public void cal()
        {//user에 편입

            lock (player_locker)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (Players[i] != null)
                    {
                        Players[i].Move();
                    }
                }
            }
        }

        public void cal_screen()
        {
            lock (player_locker)
            {
                if (Players[my_no] != null)
                {
                    screen.X = (int)(Players[my_no].x - (dot * si) / 2);
                    screen.Y = (int)(Players[my_no].y - (dot * si) / 2);
                }
            }
        }
        public void DrawPlayer()
        {//user에 편입

            Font thisfont = null;
            Brush thisbrush = new SolidBrush(Color.Black);
            thisfont = new Font("AR CENA", 10);

            lock (player_locker)
            {
                for (int i = 0; i < 7; i++)
                {
                    User player = Players[i];
                    if (player != null)
                    {
                        Point point = getDrawPoint((int)player.x, (int)player.y);
                        DoubleBuffering.Instance().getGraphics.FillRectangle(player.brush, new Rectangle(point, new Size(dot, dot)));
                        DoubleBuffering.Instance().getGraphics.DrawRectangle(new Pen(Color.Black), point.X, point.Y, dot, dot);
                    }
                }
            }
            if(Players[my_no] != null)
                DoubleBuffering.Instance().getGraphics.DrawString(Players[my_no].x + ", " + Players[my_no].y, thisfont, thisbrush, 10, 10);
        }

        public static Point getDrawPoint(int x, int y)
        {
            Point p = new Point(x - dot / 2 - screen.X, y - dot / 2 - screen.Y);
            return p;
        }
        public static PointF getDrawPoint(float x, float y)
        {
            PointF p = new PointF(x - dot / 2 - screen.X, y - dot / 2 - screen.Y);
            
            return p;
        }


    }
}
