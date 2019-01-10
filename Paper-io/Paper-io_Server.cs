using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedLibaray;
using Network;
using System.Net.Sockets;
using System.Net;

namespace Paper_io_server
{
    public partial class Paper_io_Server : Form
    {

        MasterServer MainServer = null;
        Timer_State timer = null;
        public static Queue<string> LogQueue;
        static object Queue_locker = new object();

        public Paper_io_Server()
        {
            InitializeComponent();
            LogQueue = new Queue<string>();
            MainServer = MasterServer.getinstance();
            timer = Timer_State.getinstance(this);
            timer.callback_TimerAllStopped += Form_Closing;

            lbl_IPAddress.Text = "IP : " + getLocalIP();
            lbl_Port.Text = "Port : " + MainServer.PORT;

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Start_form();
        }
        private void Start_form()
        {
            Threading_Timer thread_ServerRenew = new Threading_Timer();
            thread_ServerRenew.setCallback(Renew_Serverlist);
            thread_ServerRenew.setInterval(5000);
            thread_ServerRenew.Start();

            Threading_Timer thread_LogRecording = new Threading_Timer();
            thread_LogRecording.setCallback(Recording);
            thread_LogRecording.setInterval(100);
            thread_LogRecording.Start();
        }

        private string getLocalIP()
        {
            string myip = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myip = ip.ToString();
                    break;
                }
            }

            return myip;
        }

        private void Renew_Serverlist()
        {
            Invoke(new Action(delegate ()
            {
                lb_Serverlist.Items.Clear();
                foreach (int i in MainServer.getServerlist())
                {
                    lb_Serverlist.Items.Add(i);
                }
                
            }));

            RecordLog("Renew Serverlist");
        }


        public void Form_Closing()
        {
            RecordLog("서버 종료");
            Invoke(new Action(delegate ()
            {
                
                MainServer.Stop();
                Close();
            }));
        }

        public static void RecordLog(string str)
        {
            lock (Queue_locker)
            {
                LogQueue.Enqueue(str + "\n");
            }
        }

        private void Recording()
        {
            lock (Queue_locker)
            {
                Invoke(new Action(delegate ()
                {

                    while (LogQueue.Count != 0)
                    {
                        tb_Log.AppendText(LogQueue.Dequeue());
                    }

                }));
                int max_line = 2000;
                int delete_line = 500;
                if(tb_Log.Lines.Count() > max_line)
                {
                    string[] wow = tb_Log.Lines;
                    string[] new_log = new string[max_line - delete_line];
                    Array.Copy(wow, delete_line, new_log, 0, new_log.Length);
                    tb_Log.Lines = new_log;
                }
            }
            
        }

        

        private void lb_Serverlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListBox lb = sender as ListBox;
                if (lb.SelectedItem != null)
                {
                    GameServer gameServer = MainServer.getServer((int)lb.SelectedItem);
                    lb_Playerlist.Items.Clear();
                    foreach(User user in gameServer.Userarr)
                    {
                        if(user!=null)
                            lb_Playerlist.Items.Add(user.ID);
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                RecordLog(ex.ToString());
            }
        }
    }
}
