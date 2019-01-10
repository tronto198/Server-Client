using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Network
{
    public class Packet : ICloneable
    {
        public static readonly int HEADERSIZE = 2;
        public static readonly int PROTOCOLSIZE = 2;
        public byte[] buffer { get; private set; }
        public int position { get; private set; }
        public Int16 size { get; private set; }
        public Int16 protocol { get; private set; }


        public static Packet create(PROTOCOL protocol)
        {
            Packet packet = PacketManager.pop();
            packet.setprotocol((short)protocol);
            return packet;
        }

        public Packet(byte[] buffer)
        {
            this.buffer = new byte[1024];
            position = HEADERSIZE + PROTOCOLSIZE;
            Array.Copy(buffer,this.buffer, 1024);
            this.size = getSize();
            this.protocol = getprotocol();
        }
        public Packet()
        {
            buffer = new byte[1024];
            position = HEADERSIZE + PROTOCOLSIZE;
            size = 0;
            protocol = 0;
        }


        public void setprotocol(Int16 protocol)
        {
            this.protocol = protocol;
        }
        public Int16 getprotocol()
        {
            Int16 protocol = BitConverter.ToInt16(buffer, 2);
            return protocol;
        }
        public Int16 getSize()
        {
            Int16 size = BitConverter.ToInt16(buffer, 0);
            return size;
        }

        public void recording()
        {
            size = (Int16)(this.position - HEADERSIZE);
            byte[] header = BitConverter.GetBytes(size);
            header.CopyTo(buffer, 0);
            byte[] pheader = BitConverter.GetBytes(protocol);
            pheader.CopyTo(buffer, HEADERSIZE);
        }

        public void delete()
        {
            Array.Clear(buffer, 0, 1024);
            position = HEADERSIZE + PROTOCOLSIZE;
            protocol = 0;
            PacketManager.push(this);
        }


        public void push(long data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }
        public void push(int data)
        {
            byte[] temp_buffer = BitConverter.GetBytes(data);
            temp_buffer.CopyTo(this.buffer, this.position);
            this.position += temp_buffer.Length;
        }
        public void push(string data)
        {
            byte[] bytedata = Encoding.UTF8.GetBytes(data);
            byte len = (byte)bytedata.Length;

            buffer[this.position] = len;
            position += sizeof(byte);

            bytedata.CopyTo(buffer, position);
            position += len;
        }
        public void push(byte[] data)
        {
            Int16 len = (Int16)data.Length;
            byte[] temp_buffer = BitConverter.GetBytes(len);
            temp_buffer.CopyTo(this.buffer, this.position);
            position += sizeof(Int16);

            data.CopyTo(buffer, position);
            position += len;
        }
        public void push(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            byte[] data = ms.ToArray();
            push(data);

            
        }




        public object pop_object()
        {
            Int16 len = BitConverter.ToInt16(this.buffer, this.position);
            this.position += sizeof(Int16);

            byte[] data = new byte[len];
            Array.Copy(buffer, position, data, 0, len);
            position += len;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);

            object obj = bf.Deserialize(ms);
            return obj;
        }
        public string pop_string()
        {
            // 문자열 길이는 최대 2바이트 까지. 0 ~ 32767
            byte len = buffer[this.position];
            this.position += sizeof(byte);

            // 인코딩은 utf8로 통일한다.
            string data = System.Text.Encoding.UTF8.GetString(this.buffer, this.position, len);
            this.position += len;

            return data;
        }
        public long pop_long()
        {
            long data = BitConverter.ToInt64(this.buffer, this.position);
            position += sizeof(long);
            return data;
        }

        public int pop_int()
        {
            Int32 data = BitConverter.ToInt32(this.buffer, this.position);
            this.position += sizeof(Int32);
            return data;
        }

        public object Clone()
        {
            Packet packet = create((PROTOCOL)protocol);
            packet.position = this.position;
            packet.size = this.size;
            packet.buffer = (byte[])this.buffer.Clone();
            return packet;
        }
    }
}
