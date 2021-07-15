namespace parkingclient
{
    partial class PopupIn
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
            this.button_openbarrier = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_carplate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_openbarrier
            // 
            this.button_openbarrier.Location = new System.Drawing.Point(269, 81);
            this.button_openbarrier.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_openbarrier.Name = "button_openbarrier";
            this.button_openbarrier.Size = new System.Drawing.Size(94, 29);
            this.button_openbarrier.TabIndex = 0;
            this.button_openbarrier.Text = "Open the barrier";
            this.button_openbarrier.UseVisualStyleBackColor = true;
            this.button_openbarrier.Click += new System.EventHandler(this.Button_openbarrier_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(367, 81);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(56, 29);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.Button_cancel_Click);
            // 
            // label_carplate
            // 
            this.label_carplate.AutoSize = true;
            this.label_carplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_carplate.ForeColor = System.Drawing.Color.Green;
            this.label_carplate.Location = new System.Drawing.Point(10, 11);
            this.label_carplate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_carplate.Name = "label_carplate";
            this.label_carplate.Size = new System.Drawing.Size(0, 24);
            this.label_carplate.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "requests permission to enter";
            // 
            // PopupIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 121);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_carplate);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_openbarrier);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PopupIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entry request";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_openbarrier;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_carplate;
        private System.Windows.Forms.Label label2;
    }
}