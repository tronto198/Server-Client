using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    public class SocketConnecter
    {
        private int Maxconnect = 0;
        private int port = 0;
        public readonly int socket_per_connect = 2;
        public readonly int buffersize = 1024;

        //서버용 소켓리스너
        SocketListener socketlistener = null;

        //클라용 소켓
        Socket socket = null;

        //SocketAsyncEventArgs용 버퍼매니저
        BufferManager BufferManager = null;


        //연결 완료 핸들러
        public delegate void ConnectedHandler(Playertoken token);
        public ConnectedHandler callback_connected;

        public SocketConnecter(int Port)
        {
            this.port = Port;
            callback_connected = null;
        }
        
        public void Serverinit()
        {
            this.Maxconnect = 100;
            BufferManager = new BufferManager(Maxconnect * socket_per_connect * buffersize, buffersize);
            BufferManager.InitBuffer();
            socketlistener = new SocketListener(port);

            socketlistener.callback_newclient += new_Client;
            socketlistener.Start_listen();
        }
        public void Clientinit(IPAddress iPAddress)
        {
            this.Maxconnect = 1;
            BufferManager = new BufferManager(Maxconnect * socket_per_connect * buffersize, buffersize);
            BufferManager.InitBuffer();
            Start_connect(new IPEndPoint(iPAddress,port));
        }


        //서버용 새 클라이언트 접속시
        private void new_Client(Socket socket)
        {
            this.socket = socket;
            connect_complete(null, null);
        }

        //클라용 서버접속 시도
        private void Start_connect(IPEndPoint ip)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs event_arg = new SocketAsyncEventArgs();
            event_arg.RemoteEndPoint = ip;
            event_arg.Completed += connect_complete;
            bool pending = socket.ConnectAsync(event_arg);
            if (!pending)
            {
                connect_complete(null, event_arg);
            }
        }
        private void connect_complete(object sender, SocketAsyncEventArgs e)
        {
            Playertoken token = new Playertoken();
            SocketAsyncEventArgs Sendersocket = makeSender();
            SocketAsyncEventArgs Receiversocket = makeReceiver();
            token.setSocketAsyncEventArgs(Receiversocket, Sendersocket);
            token.setSocket(this.socket);

            Sendersocket.UserToken = token;
            Receiversocket.UserToken = token;

            if(callback_connected != null)
            {
                callback_connected(token);
            }

        }

        public void Start_Receive(Playertoken token)
        {
            /*
             * 받기 시작
             */

            bool pending = socket.ReceiveAsync(token.receive_eventargs);
            if (!pending)
            {
                Receiving(null, token.receive_eventargs);
            }
        }

        private SocketAsyncEventArgs makeReceiver()
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();


            BufferManager.SetBuffer(socketAsyncEventArgs);
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Receiving);

            return socketAsyncEventArgs;
        }
        private SocketAsyncEventArgs makeSender()
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();

            BufferManager.SetBuffer(socketAsyncEventArgs);
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Sending);

            return socketAsyncEventArgs;
        }

        private void Receiving(object sender, SocketAsyncEventArgs e)
        {
            Playertoken token = e.UserToken as Playertoken;
            if (e.SocketError == SocketError.Success)
            {
                token.receive();
                bool pending = token.socket.ReceiveAsync(e);
                if (!pending)
                {
                    Receiving(null, e);
                }
            }
            else
            {
                token.peer.disconnected();
            }
        
        }
        private void Sending(object sender, SocketAsyncEventArgs e)
        {
            Playertoken token = e.UserToken as Playertoken;
            token.Sending(e);
        }

        public void Stop()
        {
            if(socketlistener != null)
            {
                socketlistener.Close();
            }
        }
    }
}
