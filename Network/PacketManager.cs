using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public static class PacketManager
    {
        
        private static object locker = new object();
        private static Stack<Packet> pool = null;
        private static int pool_capacity = 0;

        static PacketManager()
        {
            pool = new Stack<Packet>();
            
        }

        public static void init(int capacity)
        {
            pool_capacity = capacity;
        }

        private static void allocate()
        {
            for (int i = 0; i < pool_capacity; ++i)
            {
                pool.Push(new Packet());
            }
        }

        public static Packet pop()
        {
            lock (locker)
            {
                if (pool.Count <= 0)
                {
                    //Console.WriteLine("reallocate.");
                    allocate();
                }

                return pool.Pop();
            }
        }

        public static void push(Packet packet)
        {
            lock (locker)
            {
                pool.Push(packet);
            }
        }
    }
}
