namespace parkingclient
{
    partial class PopupOut
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
            this.ButtonExitCancel = new System.Windows.Forms.Button();
            this.ButtonExitOpen = new System.Windows.Forms.Button();
            this.labelExitRequest = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonExitCancel
            // 
            this.ButtonExitCancel.Location = new System.Drawing.Point(367, 81);
            this.ButtonExitCancel.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonExitCancel.Name = "ButtonExitCancel";
            this.ButtonExitCancel.Size = new System.Drawing.Size(56, 29);
            this.ButtonExitCancel.TabIndex = 2;
            this.ButtonExitCancel.Text = "Cancel";
            this.ButtonExitCancel.UseVisualStyleBackColor = true;
            this.ButtonExitCancel.Click += new System.EventHandler(this.ButtonExitCancel_Click);
            // 
            // ButtonExitOpen
            // 
            this.ButtonExitOpen.Location = new System.Drawing.Point(267, 81);
            this.ButtonExitOpen.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonExitOpen.Name = "ButtonExitOpen";
            this.ButtonExitOpen.Size = new System.Drawing.Size(96, 29);
            this.ButtonExitOpen.TabIndex = 1;
            this.ButtonExitOpen.Text = "Open the barrier";
            this.ButtonExitOpen.UseVisualStyleBackColor = true;
            this.ButtonExitOpen.Click += new System.EventHandler(this.ButtonExitOpen_Click);
            // 
            // labelExitRequest
            // 
            this.labelExitRequest.AutoSize = true;
            this.labelExitRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelExitRequest.Location = new System.Drawing.Point(10, 47);
            this.labelExitRequest.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelExitRequest.Name = "labelExitRequest";
            this.labelExitRequest.Size = new System.Drawing.Size(233, 24);
            this.labelExitRequest.TabIndex = 4;
            this.labelExitRequest.Text = "requests permission to exit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(10, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sum is: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(91, 86);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 7;
            // 
            // PopupOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 121);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelExitRequest);
            this.Controls.Add(this.ButtonExitOpen);
            this.Controls.Add(this.ButtonExitCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopupOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exit request";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonExitCancel;
        private System.Windows.Forms.Button ButtonExitOpen;
        private System.Windows.Forms.Label labelExitRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}