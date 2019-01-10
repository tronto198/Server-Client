using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Network
{ 
    [Serializable]
    public class UserData
    {
        private static float moving_dot = 0.5f;
        public DIRECTION direction { get; private set; }
        private DIRECTION now_direction;
        public String ID { get; private set; }
        public COLOR color { get; private set; }
        public int Player_no { get; private set; }
        public float x { get; set; }
        public float y { get; set; }

        [NonSerialized]
        public SolidBrush brush;


        public delegate void MoveHandler(float x, float y);

        [NonSerialized]
        public MoveHandler callback_move = null;


        public UserData()
        {
            setID("user");
            color = 0;
            Player_no = 0;
        }
        public void setID(string ID)
        {
            this.ID = ID;
        }
        public void setCOLOR(COLOR color)
        {
            this.color = color;
            switch ((COLOR)color)
            {
                case COLOR.Blue:
                    brush = new SolidBrush(Color.Blue);
                    break;

                case COLOR.Green:
                    brush = new SolidBrush(Color.Green);
                    break;

                case COLOR.Purple:
                    brush = new SolidBrush(Color.Purple);
                    break;

                case COLOR.Red:
                    brush = new SolidBrush(Color.Red);
                    break;

                case COLOR.Yellow:
                    brush = new SolidBrush(Color.Yellow);
                    break;
                    
            }
        }
        public void setPlayer_no(int no)
        {
            this.Player_no = no;
        }
        public void setDirection(DIRECTION d)
        {
            this.direction = d;
        }

        public void Move()
        {
            if(now_direction != direction)
            {
                if(now_direction == DIRECTION.Stop)
                {
                    now_direction = direction;
                }
                else
                {

                }

            }
            now_direction = direction;
            switch ((DIRECTION)now_direction)
            {
                case DIRECTION.Up:
                    y -= moving_dot;
                    break;

                case DIRECTION.Down:
                    y += moving_dot;
                    break;

                case DIRECTION.Left:
                    x -= moving_dot;
                    break;

                case DIRECTION.Right:
                    x += moving_dot;
                    break;
            }

            if(callback_move != null)
                callback_move(x,y);
        }
        public void kill()
        {

        }

        public void sync(long ms)
        {
            for(int i = 0; i < ms/10; i++)
            {
                Move();
            }
        }
    }

    [Serializable]
    public class User : UserData
    {

        [NonSerialized]
        public Playertoken token;

        [NonSerialized]
        public IPeer Peer;
        
        
        public User()
        {
            
        }
        public void setToken(Playertoken playertoken)
        {
            this.token = playertoken;
        }
        public void setPeer(IPeer peer)
        {
            Peer = peer;
        }
        
    }
}
