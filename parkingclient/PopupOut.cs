using Npgsql;
using parkingclient.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace parkingclient
{
    public partial class PopupOut : Form
    {
        public string Id;
        public string PaymentId;
        public string Carplate;
        public string Sum;
        public bool IsLocalEvent;

        public String IdValue
        {
            get { return this.Id; }
        }

        public String PaymentIdValue
        {
            get { return this.PaymentId; }
        }

        public String CarplateValue
        {
            get { return this.label1.Text; }
        }

        public String SumValue
        {
            get { return this.label3.Text; }
        }

        public bool IsLocalEventValue
        {
            get { return this.IsLocalEvent; }
        }

        public PopupOut(String InitialIdValue, String InitialPaymentIdValue, String InitialCarplateValue, String InitialSumValue, bool InitialIsLocalEventValue)
        {
            InitializeComponent();
            this.Id = InitialIdValue;
            this.PaymentId = InitialPaymentIdValue;
            this.label1.Text = InitialCarplateValue;
            this.label3.Text = InitialSumValue;
            this.IsLocalEvent = InitialIsLocalEventValue;
        }

        private void ButtonExitCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonExitOpen_Click(object sender, EventArgs e)
        {
            if (IsLocalEventValue)
            {
                try
                {
                    string sum = "";
                    string eventid = "";

                    NpgsqlConnection connection = PostgreSQLConnection();

                    var command1 = new NpgsqlCommand { Connection = connection };
                    command1.CommandText = "UPDATE parking_invoice SET ispayed = true WHERE id = '" + this.PaymentId + "';";
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    string query2 = "SELECT sum, eventid FROM parking_invoice WHERE id = '" + this.PaymentId + "'";
                    NpgsqlCommand command2 = new NpgsqlCommand(query2, connection);
                    NpgsqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read())
                    {
                        sum = reader2.GetDouble(0).ToString();
                        eventid = reader2.GetGuid(1).ToString();
                    }

                    reader2.Close();
                    command2.Dispose();

                    sum = sum.Replace(',', '.');

                    var command3 = new NpgsqlCommand { Connection = connection };
                    command3.CommandText = "UPDATE parking_event SET exit = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', sum = " + sum + ", isclosed = true WHERE id = '" + eventid + "';";
                    command3.ExecuteNonQuery();
                    command3.Dispose();

                    var command4 = new NpgsqlCommand { Connection = connection };
                    command4.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                        "WHERE id = '" + Main.exitBarrier + "';";
                    command4.ExecuteNonQuery();
                    command4.Dispose();

                    connection.Close();
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg.ToString());
                }
            }
            else
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + Main.apiUrl + "exitevent/" + this.PaymentId);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    if (content != "") { }

                    NpgsqlConnection connection = PostgreSQLConnection();
                    var command = new NpgsqlCommand { Connection = connection };
                    command.CommandText = "UPDATE parking_barrier SET needtoopen = true WHERE id = '" + Main.exitBarrier + "';";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg.ToString());
                }
            }

            this.Close();
        }

        public NpgsqlConnection PostgreSQLConnection()
        {
            try
            {
                string DatabaseHost = (string)Settings.Default["DatabaseHost"];
                string DatabasePort = (string)Settings.Default["DatabasePort"];
                string DatabaseName = (string)Settings.Default["DatabaseName"];
                string DatabaseUser = (string)Settings.Default["DatabaseUser"];
                string DatabasePass = (string)Settings.Default["DatabasePass"];

                if (!String.IsNullOrEmpty(DatabaseHost) &&
                    !String.IsNullOrEmpty(DatabasePort) &&
                    !String.IsNullOrEmpty(DatabaseUser) &&
                    !String.IsNullOrEmpty(DatabasePass) &&
                    !String.IsNullOrEmpty(DatabaseName))
                {
                    var cs = "Host=" + DatabaseHost + ";" +
                        "Port=" + DatabasePort + ";" +
                        "Username=" + DatabaseUser + ";" +
                        "Password=" + DatabasePass + ";" +
                        "Database=" + DatabaseName + ";";

                    var con = new NpgsqlConnection(cs);
                    con.Open();

                    return con;
                }
                return null;
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.ToString());
                throw;
            }
        }
    }
}
