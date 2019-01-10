using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class Packetmaker
    {
        public delegate void callback_complete(Packet packet);

        byte[] buffer = new byte[1024];
        int Packet_size = 0;
        int remain_byte = 0;
        int position = 0;
        int target = 0;

        

        public void unPacking(byte[] buffer, int offset, int transferred, callback_complete callback)
        {
            
            remain_byte = transferred;
            int or_position = offset;

            while(remain_byte > 0)
            {
                bool complete = false;

                if (position < Packet.HEADERSIZE)
                {
                    target = Packet.HEADERSIZE;
                    complete = Reading(buffer, ref or_position, offset, transferred);

                    if (!complete)
                    {
                        return;
                    }

                    Packet_size = get_size();

                    target = Packet_size + Packet.HEADERSIZE;
                }

                complete = Reading(buffer, ref or_position, offset, transferred);

                if (complete)
                {
                    callback(new Packet(this.buffer));
                    buffer_clear();
                }

            }
            
        }

        private bool Reading(byte[] buffer, ref int or_position, int offset, int transffered)
        {
            if(position >= offset + transffered)
            {
                return false;
            }

            int size = target - position;

            if(remain_byte < size)
            {
                size = remain_byte;
            }

            Array.Copy(buffer, or_position, this.buffer, this.position, size);

            or_position += size;
            position += size;
            remain_byte -= size;

            if(position < target)
            {
                return false;
            }

            return true;
        }

        private Int16 get_size()
        {
            return BitConverter.ToInt16(buffer, 0);
        }
        private void buffer_clear()
        {
            Array.Clear(buffer, 0, Packet_size);
            position = 0;
            Packet_size = 0;
        }
    }
}
