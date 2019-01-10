using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Network
{
    public class SocketListener
    {

        private int PORT = 0;
        public readonly int BACKLOG = 30;
        

        //비동기 accept eventargs
        SocketAsyncEventArgs accept_args = null;
        Socket Server_socket = null;


        //새 클라이언트 접속시 콜백
        public delegate void NewclientHandler(Socket Client_socket);
        public NewclientHandler callback_newclient;


        //순서조절
        AutoResetEvent Flow_control = null;
        //접속 감지 스레드
        Thread thread_Listening = null;


        public SocketListener(int port)
        {
            this.PORT = port;
            callback_newclient = null;
            Bind();
        }

        private void Bind()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT);
            Server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Server_socket.Bind(ip);
                accept_args = new SocketAsyncEventArgs();
                accept_args.Completed += new EventHandler<SocketAsyncEventArgs>(accept_complete);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        
        private void Listening()
        {
            while (true)
            {
                accept_args.AcceptSocket = null;
                bool pending = true;
                try
                {
                    Server_socket.Listen(30);
                    pending = Server_socket.AcceptAsync(accept_args);

                }
                catch (Exception e)
                {
                    throw (e);
                }

                if (!pending)
                {
                    accept_complete(null, accept_args);
                }

                Flow_control.WaitOne();
            }
        }

        public void Start_listen()
        {
            Flow_control = new AutoResetEvent(false);
            thread_Listening = new Thread(Listening);
            thread_Listening.Start();
        }

        public void Close()
        {
            if(thread_Listening != null)
                thread_Listening.Abort();
        }

        private void accept_complete(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket client_socket = e.AcceptSocket;

                if (callback_newclient != null)
                {
                    callback_newclient(client_socket);
                }
            }
            Flow_control.Set();
        }


    }
}
