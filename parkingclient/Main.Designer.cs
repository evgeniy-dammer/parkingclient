namespace parkingclient
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.allEventsDataGridView = new System.Windows.Forms.DataGridView();
            this.ButtonPrint = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.printDialogQRCode = new System.Windows.Forms.PrintDialog();
            this.printDocumentQRCode = new System.Drawing.Printing.PrintDocument();
            this.comboBoxCarType = new System.Windows.Forms.ComboBox();
            this.comboBoxNationalityType = new System.Windows.Forms.ComboBox();
            this.textBoxReader = new System.Windows.Forms.TextBox();
            this.labelCarplate = new System.Windows.Forms.Label();
            this.buttonCloseEntryBarrier = new System.Windows.Forms.Button();
            this.buttonCloseExitBarrier = new System.Windows.Forms.Button();
            this.buttonOpenEntryBarrier = new System.Windows.Forms.Button();
            this.buttonOpenExitBarrier = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.allEventsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // allEventsDataGridView
            // 
            this.allEventsDataGridView.AllowUserToOrderColumns = true;
            this.allEventsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.allEventsDataGridView, "allEventsDataGridView");
            this.allEventsDataGridView.Name = "allEventsDataGridView";
            this.allEventsDataGridView.RowTemplate.Height = 24;
            // 
            // ButtonPrint
            // 
            this.ButtonPrint.BackColor = System.Drawing.Color.PaleGoldenrod;
            resources.ApplyResources(this.ButtonPrint, "ButtonPrint");
            this.ButtonPrint.Name = "ButtonPrint";
            this.ButtonPrint.UseVisualStyleBackColor = false;
            this.ButtonPrint.Click += new System.EventHandler(this.ButtonPrint_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // printDialogQRCode
            // 
            this.printDialogQRCode.UseEXDialog = true;
            // 
            // comboBoxCarType
            // 
            this.comboBoxCarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBoxCarType, "comboBoxCarType");
            this.comboBoxCarType.Name = "comboBoxCarType";
            // 
            // comboBoxNationalityType
            // 
            this.comboBoxNationalityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBoxNationalityType, "comboBoxNationalityType");
            this.comboBoxNationalityType.Name = "comboBoxNationalityType";
            // 
            // textBoxReader
            // 
            resources.ApplyResources(this.textBoxReader, "textBoxReader");
            this.textBoxReader.Name = "textBoxReader";
            // 
            // labelCarplate
            // 
            resources.ApplyResources(this.labelCarplate, "labelCarplate");
            this.labelCarplate.Name = "labelCarplate";
            // 
            // buttonCloseEntryBarrier
            // 
            this.buttonCloseEntryBarrier.BackColor = System.Drawing.Color.LightCoral;
            this.buttonCloseEntryBarrier.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonCloseEntryBarrier, "buttonCloseEntryBarrier");
            this.buttonCloseEntryBarrier.Name = "buttonCloseEntryBarrier";
            this.buttonCloseEntryBarrier.UseVisualStyleBackColor = false;
            this.buttonCloseEntryBarrier.Click += new System.EventHandler(this.ButtonCloseEntryBarrier_Click);
            // 
            // buttonCloseExitBarrier
            // 
            this.buttonCloseExitBarrier.BackColor = System.Drawing.Color.LightCoral;
            this.buttonCloseExitBarrier.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonCloseExitBarrier, "buttonCloseExitBarrier");
            this.buttonCloseExitBarrier.Name = "buttonCloseExitBarrier";
            this.buttonCloseExitBarrier.UseVisualStyleBackColor = false;
            this.buttonCloseExitBarrier.Click += new System.EventHandler(this.ButtonCloseExitBarrier_Click);
            // 
            // buttonOpenEntryBarrier
            // 
            this.buttonOpenEntryBarrier.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.buttonOpenEntryBarrier.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonOpenEntryBarrier, "buttonOpenEntryBarrier");
            this.buttonOpenEntryBarrier.Name = "buttonOpenEntryBarrier";
            this.buttonOpenEntryBarrier.UseVisualStyleBackColor = false;
            this.buttonOpenEntryBarrier.Click += new System.EventHandler(this.ButtonOpenEntryBarrier_Click);
            // 
            // buttonOpenExitBarrier
            // 
            this.buttonOpenExitBarrier.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.buttonOpenExitBarrier.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonOpenExitBarrier, "buttonOpenExitBarrier");
            this.buttonOpenExitBarrier.Name = "buttonOpenExitBarrier";
            this.buttonOpenExitBarrier.UseVisualStyleBackColor = false;
            this.buttonOpenExitBarrier.Click += new System.EventHandler(this.ButtonOpenExitBarrier_Click);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.ButtonSettings_Click);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonOpenExitBarrier);
            this.Controls.Add(this.buttonOpenEntryBarrier);
            this.Controls.Add(this.buttonCloseExitBarrier);
            this.Controls.Add(this.buttonCloseEntryBarrier);
            this.Controls.Add(this.labelCarplate);
            this.Controls.Add(this.textBoxReader);
            this.Controls.Add(this.comboBoxNationalityType);
            this.Controls.Add(this.comboBoxCarType);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ButtonPrint);
            this.Controls.Add(this.allEventsDataGridView);
            this.Name = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.allEventsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView allEventsDataGridView;
        private System.Windows.Forms.Button ButtonPrint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PrintDialog printDialogQRCode;
        private System.Drawing.Printing.PrintDocument printDocumentQRCode;
        private System.Windows.Forms.ComboBox comboBoxCarType;
        private System.Windows.Forms.ComboBox comboBoxNationalityType;
        private System.Windows.Forms.TextBox textBoxReader;
        private System.Windows.Forms.Label labelCarplate;
        private System.Windows.Forms.Button buttonCloseEntryBarrier;
        private System.Windows.Forms.Button buttonCloseExitBarrier;
        private System.Windows.Forms.Button buttonOpenEntryBarrier;
        private System.Windows.Forms.Button buttonOpenExitBarrier;
        private System.Windows.Forms.Button buttonSettings;
    }
}

