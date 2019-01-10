using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Network
{
    public class Playertoken
    {
        public Socket socket { get; private set; }
        public IPeer peer { get; private set; }

        public SocketAsyncEventArgs receive_eventargs { get; private set; }
        public SocketAsyncEventArgs send_eventargs { get; private set; }

        Packetmaker packetmaker = new Packetmaker();

        Queue<Packet> sending_queue = null;
        object queue_locker = new object();

        public Playertoken()
        {
            sending_queue = new Queue<Packet>();
            socket = null;
            peer = null;
            receive_eventargs = null;
            send_eventargs = null;
        }
        public void setSocket(Socket s)
        {
            socket = s;
        }
        public void setIPeer(IPeer peer)
        {
            this.peer = peer;
        }
        public void setSocketAsyncEventArgs(SocketAsyncEventArgs receiver, SocketAsyncEventArgs sender)
        {
            receive_eventargs = receiver;
            send_eventargs = sender;
        }

        public void send(Packet packet)
        {
            packet.recording();
            lock (queue_locker)
            {
                sending_queue.Enqueue(packet);
                if (sending_queue.Count == 1)
                {
                    send_start();
                }
            }
            
        }
        private void send_start()
        {
            lock (queue_locker)
            {
                Packet packet = sending_queue.Peek();
                
                send_eventargs.SetBuffer(send_eventargs.Offset, packet.position);

                Array.Copy(packet.buffer, 0, send_eventargs.Buffer, send_eventargs.Offset, packet.position);

                bool pending = socket.SendAsync(send_eventargs);
                if (!pending)
                {
                    Sending(send_eventargs);
                }
            }
        }
        public void Sending(SocketAsyncEventArgs e)
        {
            ///여기 다시 보기
            ///
            if (send_eventargs.BytesTransferred <= 0 || send_eventargs.SocketError != SocketError.Success)
            {
                return;
            }

            lock (queue_locker)
            {
                int size = sending_queue.Peek().position;
                if (e.BytesTransferred != size)
                {
                    e.SetBuffer(e.BytesTransferred, size - e.BytesTransferred);
                    bool pending = socket.SendAsync(e);
                    if (!pending)
                    {
                        Sending(send_eventargs);
                    }

                    return;
                }

                Packet packet = sending_queue.Dequeue();
                packet.delete();

            }
            if (this.sending_queue.Count > 0)
            {
                send_start();
            }

        }

        public void receive()
        {
            byte[] buffer = receive_eventargs.Buffer;
            int offset = receive_eventargs.Offset;
            int transferred = receive_eventargs.BytesTransferred;
            packetmaker.unPacking(buffer, offset, transferred, Receive_complete);
        }

        private void Receive_complete(Packet packet)
        {
            peer.Receive(packet);
        }
    }
}
