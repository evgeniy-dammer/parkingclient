using System;
using System.Configuration;
using System.Windows.Forms;
using parkingclient.Properties;

namespace parkingclient
{
    public partial class AppSettings : Form
    {
        public AppSettings()
        {
            InitializeComponent();
            FillSettings();
        }

        private void FillSettings()
        {
            try
            {
                textBoxParkingId.Text = (string)Settings.Default["ParkingId"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxDatabaseHost.Text = (string)Settings.Default["DatabaseHost"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxDatabasePort.Text = (string)Settings.Default["DatabasePort"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxDatabaseName.Text = (string)Settings.Default["DatabaseName"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxDatabaseUser.Text = (string)Settings.Default["DatabaseUser"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxDatabasePass.Text = (string)Settings.Default["DatabasePass"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxComPort.Text = (string)Settings.Default["ComPort"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxEntryBarrierId.Text = (string)Settings.Default["EntryBarrierId"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxExitBarrierId.Text = (string)Settings.Default["ExitBarrierId"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxApiUrl.Text = (string)Settings.Default["ApiUrl"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }

            try
            {
                textBoxSound.Text = (string)Settings.Default["Sound"];
            }
            catch (SettingsPropertyNotFoundException ex) { Console.WriteLine(ex.ToString()); }
        }

        private void TextBoxDatabaseHost_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["DatabaseHost"] = textBoxDatabaseHost.Text;
            Settings.Default.Save();
        }

        private void TextBoxDatabasePort_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["DatabasePort"] = textBoxDatabasePort.Text;
            Settings.Default.Save();
        }

        private void TextBoxDatabaseName_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["DatabaseName"] = textBoxDatabaseName.Text;
            Settings.Default.Save();
        }

        private void TextBoxDatabaseUser_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["DatabaseUser"] = textBoxDatabaseUser.Text;
            Settings.Default.Save();
        }

        private void TextBoxDatabasePass_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["DatabasePass"] = textBoxDatabasePass.Text;
            Settings.Default.Save();
        }

        private void TextBoxParkingId_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["ParkingId"] = textBoxParkingId.Text;
            Settings.Default.Save();
        }

        private void TextBoxComPort_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["ComPort"] = textBoxComPort.Text;
            Settings.Default.Save();
        }

        private void TextBoxEntryBarrierId_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["EntryBarrierId"] = textBoxEntryBarrierId.Text;
            Settings.Default.Save();
        }

        private void TextBoxExitBarrierId_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["ExitBarrierId"] = textBoxExitBarrierId.Text;
            Settings.Default.Save();
        }

        private void TextBoxApiUrl_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["ApiUrl"] = textBoxApiUrl.Text;
            Settings.Default.Save();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["Sound"] = textBoxSound.Text;
            Settings.Default.Save();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
