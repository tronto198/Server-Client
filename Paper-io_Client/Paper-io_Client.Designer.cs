namespace Paper_io_Client
{
    partial class Paper_io_Client
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
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.lbl_ID = new System.Windows.Forms.Label();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_IP
            // 
            this.tb_IP.CausesValidation = false;
            this.tb_IP.Location = new System.Drawing.Point(336, 401);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(195, 25);
            this.tb_IP.TabIndex = 0;
            this.tb_IP.TabStop = false;
            this.tb_IP.Text = "127.0.0.1";
            // 
            // button1
            // 
            this.button1.CausesValidation = false;
            this.button1.Location = new System.Drawing.Point(555, 364);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 51);
            this.button1.TabIndex = 1;
            this.button1.TabStop = false;
            this.button1.Text = "접속";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Connect_Button_Click);
            // 
            // tb_id
            // 
            this.tb_id.CausesValidation = false;
            this.tb_id.Location = new System.Drawing.Point(336, 364);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(195, 25);
            this.tb_id.TabIndex = 2;
            this.tb_id.TabStop = false;
            // 
            // lbl_ID
            // 
            this.lbl_ID.AutoSize = true;
            this.lbl_ID.Location = new System.Drawing.Point(300, 364);
            this.lbl_ID.Name = "lbl_ID";
            this.lbl_ID.Size = new System.Drawing.Size(30, 15);
            this.lbl_ID.TabIndex = 3;
            this.lbl_ID.Text = "ID :";
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(264, 400);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(66, 15);
            this.lbl_IP.TabIndex = 4;
            this.lbl_IP.Text = "Host IP :";
            // 
            // Paper_io_Client
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(882, 853);
            this.Controls.Add(this.lbl_IP);
            this.Controls.Add(this.lbl_ID);
            this.Controls.Add(this.tb_id);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_IP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Paper_io_Client";
            this.Text = "Paper-io";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_IP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_id;
        private System.Windows.Forms.Label lbl_ID;
        private System.Windows.Forms.Label lbl_IP;





    }
}

