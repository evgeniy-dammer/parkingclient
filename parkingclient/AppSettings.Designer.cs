namespace parkingclient
{
    partial class AppSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEntryBarrierId = new System.Windows.Forms.TextBox();
            this.textBoxComPort = new System.Windows.Forms.TextBox();
            this.textBoxParkingId = new System.Windows.Forms.TextBox();
            this.textBoxDatabasePass = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseUser = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseName = new System.Windows.Forms.TextBox();
            this.textBoxDatabasePort = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseHost = new System.Windows.Forms.TextBox();
            this.textBoxExitBarrierId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxApiUrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxSound = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Entry Barrier ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "COM Port:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 14);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Parking ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Database Pass:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Database User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Database Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Database Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Database Host:";
            // 
            // textBoxEntryBarrierId
            // 
            this.textBoxEntryBarrierId.Location = new System.Drawing.Point(122, 192);
            this.textBoxEntryBarrierId.Name = "textBoxEntryBarrierId";
            this.textBoxEntryBarrierId.Size = new System.Drawing.Size(270, 20);
            this.textBoxEntryBarrierId.TabIndex = 9;
            this.textBoxEntryBarrierId.TextChanged += new System.EventHandler(this.TextBoxEntryBarrierId_TextChanged);
            // 
            // textBoxComPort
            // 
            this.textBoxComPort.Location = new System.Drawing.Point(122, 166);
            this.textBoxComPort.Name = "textBoxComPort";
            this.textBoxComPort.Size = new System.Drawing.Size(270, 20);
            this.textBoxComPort.TabIndex = 8;
            this.textBoxComPort.TextChanged += new System.EventHandler(this.TextBoxComPort_TextChanged);
            // 
            // textBoxParkingId
            // 
            this.textBoxParkingId.Location = new System.Drawing.Point(122, 11);
            this.textBoxParkingId.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxParkingId.Name = "textBoxParkingId";
            this.textBoxParkingId.Size = new System.Drawing.Size(270, 20);
            this.textBoxParkingId.TabIndex = 2;
            this.textBoxParkingId.TextChanged += new System.EventHandler(this.TextBoxParkingId_TextChanged);
            // 
            // textBoxDatabasePass
            // 
            this.textBoxDatabasePass.Location = new System.Drawing.Point(122, 140);
            this.textBoxDatabasePass.Name = "textBoxDatabasePass";
            this.textBoxDatabasePass.PasswordChar = '*';
            this.textBoxDatabasePass.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatabasePass.TabIndex = 7;
            this.textBoxDatabasePass.TextChanged += new System.EventHandler(this.TextBoxDatabasePass_TextChanged);
            // 
            // textBoxDatabaseUser
            // 
            this.textBoxDatabaseUser.Location = new System.Drawing.Point(122, 114);
            this.textBoxDatabaseUser.Name = "textBoxDatabaseUser";
            this.textBoxDatabaseUser.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatabaseUser.TabIndex = 6;
            this.textBoxDatabaseUser.TextChanged += new System.EventHandler(this.TextBoxDatabaseUser_TextChanged);
            // 
            // textBoxDatabaseName
            // 
            this.textBoxDatabaseName.Location = new System.Drawing.Point(122, 88);
            this.textBoxDatabaseName.Name = "textBoxDatabaseName";
            this.textBoxDatabaseName.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatabaseName.TabIndex = 5;
            this.textBoxDatabaseName.TextChanged += new System.EventHandler(this.TextBoxDatabaseName_TextChanged);
            // 
            // textBoxDatabasePort
            // 
            this.textBoxDatabasePort.Location = new System.Drawing.Point(122, 62);
            this.textBoxDatabasePort.Name = "textBoxDatabasePort";
            this.textBoxDatabasePort.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatabasePort.TabIndex = 4;
            this.textBoxDatabasePort.TextChanged += new System.EventHandler(this.TextBoxDatabasePort_TextChanged);
            // 
            // textBoxDatabaseHost
            // 
            this.textBoxDatabaseHost.Location = new System.Drawing.Point(122, 36);
            this.textBoxDatabaseHost.Name = "textBoxDatabaseHost";
            this.textBoxDatabaseHost.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatabaseHost.TabIndex = 3;
            this.textBoxDatabaseHost.TextChanged += new System.EventHandler(this.TextBoxDatabaseHost_TextChanged);
            // 
            // textBoxExitBarrierId
            // 
            this.textBoxExitBarrierId.Location = new System.Drawing.Point(122, 218);
            this.textBoxExitBarrierId.Name = "textBoxExitBarrierId";
            this.textBoxExitBarrierId.Size = new System.Drawing.Size(270, 20);
            this.textBoxExitBarrierId.TabIndex = 10;
            this.textBoxExitBarrierId.TextChanged += new System.EventHandler(this.TextBoxExitBarrierId_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 221);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Exit Barrier ID:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "API Url:";
            // 
            // textBoxApiUrl
            // 
            this.textBoxApiUrl.Location = new System.Drawing.Point(122, 244);
            this.textBoxApiUrl.Name = "textBoxApiUrl";
            this.textBoxApiUrl.Size = new System.Drawing.Size(270, 20);
            this.textBoxApiUrl.TabIndex = 11;
            this.textBoxApiUrl.TextChanged += new System.EventHandler(this.TextBoxApiUrl_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 273);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "Sound:";
            // 
            // textBoxSound
            // 
            this.textBoxSound.Location = new System.Drawing.Point(122, 270);
            this.textBoxSound.Name = "textBoxSound";
            this.textBoxSound.Size = new System.Drawing.Size(270, 20);
            this.textBoxSound.TabIndex = 12;
            this.textBoxSound.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(14, 297);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(378, 40);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // AppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(404, 346);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxSound);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxApiUrl);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxExitBarrierId);
            this.Controls.Add(this.textBoxEntryBarrierId);
            this.Controls.Add(this.textBoxComPort);
            this.Controls.Add(this.textBoxParkingId);
            this.Controls.Add(this.textBoxDatabasePass);
            this.Controls.Add(this.textBoxDatabaseUser);
            this.Controls.Add(this.textBoxDatabaseName);
            this.Controls.Add(this.textBoxDatabasePort);
            this.Controls.Add(this.textBoxDatabaseHost);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEntryBarrierId;
        private System.Windows.Forms.TextBox textBoxComPort;
        private System.Windows.Forms.TextBox textBoxParkingId;
        private System.Windows.Forms.TextBox textBoxDatabasePass;
        private System.Windows.Forms.TextBox textBoxDatabaseUser;
        private System.Windows.Forms.TextBox textBoxDatabaseName;
        private System.Windows.Forms.TextBox textBoxDatabasePort;
        private System.Windows.Forms.TextBox textBoxDatabaseHost;
        private System.Windows.Forms.TextBox textBoxExitBarrierId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxApiUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxSound;
        private System.Windows.Forms.Button buttonClose;
    }
}