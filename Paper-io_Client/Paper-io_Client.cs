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

namespace Paper_io_Client
{
    public partial class Paper_io_Client : Form
    {
        private Timer_State timer;
        MainProgram program;
        public delegate void drawHandler();
        public static drawHandler callback_Draw;

        public Paper_io_Client()
        {
            InitializeComponent();
            timer = Timer_State.getinstance();
            timer.callback_TimerAllStopped += Formclose;
            program = MainProgram.getinstance();
            
            DoubleBufferingSetting();

            this.ActiveControl = tb_id;
            Form_input.binding(this);
        }

        private void Formclose()
        {
            Invoke(new Action(delegate ()
            {
                program.Close();
                Close();
            }));

        }

        public void DoubleBufferingSetting()
        {
            Graphics gg = CreateGraphics();
            DoubleBuffering.Instance(BufferedGraphicsManager.Current.Allocate(gg, this.ClientRectangle));
            gg.Dispose();

            void Render()
            {
                try
                {
                    DoubleBuffering.Work();

                    Invoke(new Action(delegate ()
                    {
                        try
                        {
                            Graphics g = CreateGraphics();
                            DoubleBuffering.Instance().getBuffered.Render(g);
                            g.Dispose();
                        }
                        catch(Exception e)
                        {

                        }
                    }));

                }
                catch(Exception e)
                {
                    
                }
            }

            Threading_Timer thread_FrameRender = new Threading_Timer();
            thread_FrameRender.setCallback(new Action(delegate() {
                //callback_Draw();
                Render();
            }));
            thread_FrameRender.setInterval(8);
            thread_FrameRender.Start();

        }

        private void Connect_Button_Click(object sender, EventArgs e)
        {
            string ip = tb_IP.Text;
            string id = tb_id.Text;
            try
            {
                if(id.Length < 2 || id.Length > 10)
                {
                    throw new Exception("ID는 2~10자리 이내여야 합니다.");
                }
                program.Connect_start(ip,id);

                tb_IP.Visible = false;
                tb_id.Dispose();
                

                tb_id.Visible = false;
                tb_id.Dispose();

                button1.Visible = false;
                button1.Parent.Focus();

                lbl_ID.Visible = false;
               

                lbl_IP.Visible = false;
                
                this.Focus();
                
                while(!this.Focused)
                {
                    SendKeys.Send("{TAB}");
                }

                if(!this.Focused)
                {
                    object obj = this;

                    if (tb_id.Focused)
                        obj = tb_id;
                    else if (tb_IP.Focused)
                        obj = tb_IP;
                    else if (button1.Focused)
                        obj = button1;
                    else if (lbl_ID.Focused)
                        obj = lbl_ID;
                    else if (lbl_IP.Focused)
                        obj = lbl_IP;

                    throw new Exception(obj.ToString() + "  focused");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
