using System;

namespace Network
{
    [Serializable]
    public enum PROTOCOL : Int16
    {

        Error = 0,


        Request = 100,

        Request_Change_Direction = Request + 1,
        Request_Change_ID = Request + 2,


        Request_Client_Exit = Request + 11,
        Request_Client_Pause = Request  + 19,



        Accept = 200,
        Accept_Change_Direction = Accept + 1,
        Accept_Chage_ID = Accept + 2,



        Notification = 300,
        Note_Servertime = Notification + 1,




        Player = 400,
        Player_ID = Player + 1,
        Player_no = Player + 2,
        Player_exit = Player + 3,


        
        
        
    }

    [Serializable]
    public enum COLOR : Int16
    {
        Red = 1,
        Blue = 2,
        Green = 3,
        Purple = 4,
        Yellow = 5
    }

    [Serializable]
    public enum DIRECTION : Int16
    {
        Stop = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
}