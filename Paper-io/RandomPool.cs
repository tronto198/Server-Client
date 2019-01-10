using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_io_server
{
    class RandomPool
    {
        Random random;
        int Minimum = 0;
        int Maximum = 0;
        bool[] Pool = null;
        int Remain = 0;
        object Pool_locker = new object();

        public RandomPool(int min, int max)
        {
            random = new Random();
            Minimum = min;
            Maximum = max;
            Remain = Maximum - Minimum + 1;
            Pool = new bool[Remain];
            for(int i = 0; i < Remain; i++)
            {
                Pool[i] = false;
            }
            
        }

        public int pop()
        {
            lock (Pool_locker)
            {
                if (Remain == 0)
                {
                    throw new Exception("Pool Remain 0");

                }

                int no = random.Next(Minimum, Maximum + 1);
                while (Pool[no - Minimum])
                {
                    no--;
                    if (no < Minimum)
                    {
                        no = Maximum;
                    }
                }

                Pool[no - Minimum] = true;
                Remain--;
                return no;
            }
        }
        public void push(int no)
        {
            lock (Pool_locker)
            {
                Pool[no - Minimum] = false;
                Remain++;
            }
        }
    }

}
