namespace Paper_io_server
{
    partial class Paper_io_Server
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Serverlist = new System.Windows.Forms.ListBox();
            this.lb_Playerlist = new System.Windows.Forms.ListBox();
            this.lbl_Servertime = new System.Windows.Forms.Label();
            this.lbl_IPAddress = new System.Windows.Forms.Label();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.tb_Log = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lb_Serverlist
            // 
            this.lb_Serverlist.FormattingEnabled = true;
            this.lb_Serverlist.ItemHeight = 15;
            this.lb_Serverlist.Location = new System.Drawing.Point(44, 108);
            this.lb_Serverlist.Name = "lb_Serverlist";
            this.lb_Serverlist.Size = new System.Drawing.Size(165, 214);
            this.lb_Serverlist.TabIndex = 0;
            this.lb_Serverlist.SelectedIndexChanged += new System.EventHandler(this.lb_Serverlist_SelectedIndexChanged);
            // 
            // lb_Playerlist
            // 
            this.lb_Playerlist.FormattingEnabled = true;
            this.lb_Playerlist.ItemHeight = 15;
            this.lb_Playerlist.Location = new System.Drawing.Point(270, 153);
            this.lb_Playerlist.Name = "lb_Playerlist";
            this.lb_Playerlist.Size = new System.Drawing.Size(159, 169);
            this.lb_Playerlist.TabIndex = 1;
            // 
            // lbl_Servertime
            // 
            this.lbl_Servertime.AutoSize = true;
            this.lbl_Servertime.Font = new System.Drawing.Font("굴림", 11F);
            this.lbl_Servertime.Location = new System.Drawing.Point(267, 108);
            this.lbl_Servertime.Name = "lbl_Servertime";
            this.lbl_Servertime.Size = new System.Drawing.Size(94, 19);
            this.lbl_Servertime.TabIndex = 2;
            this.lbl_Servertime.Text = "servertime";
            // 
            // lbl_IPAddress
            // 
            this.lbl_IPAddress.AutoSize = true;
            this.lbl_IPAddress.Font = new System.Drawing.Font("굴림", 11F);
            this.lbl_IPAddress.Location = new System.Drawing.Point(41, 28);
            this.lbl_IPAddress.Name = "lbl_IPAddress";
            this.lbl_IPAddress.Size = new System.Drawing.Size(116, 19);
            this.lbl_IPAddress.TabIndex = 3;
            this.lbl_IPAddress.Text = "lbl_IPAddress";
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Font = new System.Drawing.Font("굴림", 11F);
            this.lbl_Port.Location = new System.Drawing.Point(41, 58);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(67, 19);
            this.lbl_Port.TabIndex = 4;
            this.lbl_Port.Text = "lbl_Port";
            // 
            // tb_Log
            // 
            this.tb_Log.Cursor = System.Windows.Forms.Cursors.Default;
            this.tb_Log.Location = new System.Drawing.Point(44, 350);
            this.tb_Log.Multiline = true;
            this.tb_Log.Name = "tb_Log";
            this.tb_Log.ReadOnly = true;
            this.tb_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Log.Size = new System.Drawing.Size(390, 97);
            this.tb_Log.TabIndex = 5;
            // 
            // Paper_io_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 486);
            this.Controls.Add(this.tb_Log);
            this.Controls.Add(this.lbl_Port);
            this.Controls.Add(this.lbl_IPAddress);
            this.Controls.Add(this.lbl_Servertime);
            this.Controls.Add(this.lb_Playerlist);
            this.Controls.Add(this.lb_Serverlist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Paper_io_Server";
            this.Text = "Paper-io_server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_Serverlist;
        private System.Windows.Forms.ListBox lb_Playerlist;
        private System.Windows.Forms.Label lbl_Servertime;
        private System.Windows.Forms.Label lbl_IPAddress;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.TextBox tb_Log;
    }
}

