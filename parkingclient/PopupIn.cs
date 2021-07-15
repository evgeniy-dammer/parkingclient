using Npgsql;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace parkingclient
{
    public partial class PopupIn : Form
    {
        public static string apiUrl = "http://192.168.0.153:8181/api/";
        public string Id;
        public string Carplate;
        public bool IsLocalEvent;

        public String IdValue
        {
            get{return this.Id;}
        }

        public String CarplateValue
        {
            get{return this.label_carplate.Text;}
        }

        public bool IsLocalEventValue
        {
            get { return this.IsLocalEvent; }
        }

        public PopupIn(String InitialIdValue, String InitialCarplateValue, bool InitialIsLocalEventValue)
        {
            InitializeComponent();
            this.Id = InitialIdValue;
            this.label_carplate.Text = InitialCarplateValue;
            this.IsLocalEvent = InitialIsLocalEventValue;
        }

        private void Button_cancel_Click(object sender, EventArgs e)
        {
            if (IsLocalEventValue)
            {
                try
                {
                    NpgsqlConnection connection = PostgreSQLConnection();
                    var command1 = new NpgsqlCommand { Connection = connection };
                    command1.CommandText = "UPDATE parking_event SET " +
                                                "isshown = true, " +
                                                "isdenied = true " +
                                                "WHERE id = '" + this.Id + "';";
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    string BarrierId = "90f38534-8acc-11eb-a401-9d3cfc2ca371";
                    var command2 = new NpgsqlCommand { Connection = connection };
                    command2.CommandText = "UPDATE parking_barrier SET needtoopen = false " +
                                        "WHERE id = '" + BarrierId + "';";
                    command2.ExecuteNonQuery();
                    command2.Dispose();

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
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "denied/" + this.Id);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    if (content != "") {}

                    string BarrierId = "90f38534-8acc-11eb-a401-9d3cfc2ca371";

                    NpgsqlConnection connection = PostgreSQLConnection();
                    var command2 = new NpgsqlCommand { Connection = connection };
                    command2.CommandText = "UPDATE parking_barrier SET needtoopen = false " +
                                        "WHERE id = '" + BarrierId + "';";
                    command2.ExecuteNonQuery();
                    command2.Dispose();

                    connection.Close();
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg.ToString());
                }
            }

            this.Close();
        }

        private void Button_openbarrier_Click(object sender, EventArgs e)
        {
            if (IsLocalEventValue)
            {
                try
                {
                    string sum = "";
                    string tariff = "";
                    NpgsqlConnection connection = PostgreSQLConnection();

                    string query1 = "SELECT tariff FROM parking_event WHERE id = '" + this.Id + "'";
                    NpgsqlCommand command1 = new NpgsqlCommand(query1, connection);
                    NpgsqlDataReader reader1 = command1.ExecuteReader();

                    while (reader1.Read())
                    {
                        tariff = reader1.GetGuid(0).ToString();
                    }

                    reader1.Close();
                    command1.Dispose();

                    string query2 = "SELECT entry FROM parking_tariff WHERE id = '" + tariff + "'";
                    NpgsqlCommand command2 = new NpgsqlCommand(query2, connection);
                    NpgsqlDataReader reader2 = command2.ExecuteReader();

                    while (reader1.Read())
                    {
                        sum = reader2.GetDouble(0).ToString().Replace(',', '.');
                    }

                    reader2.Close();
                    command2.Dispose();

                    var command3 = new NpgsqlCommand { Connection = connection };
                    command3.CommandText = "UPDATE parking_event SET " +
                                                "sum = " + sum + ", " +
                                                "isshown = true, " +
                                                "isaccepted = true " +
                                                "WHERE id = '" + this.Id + "';";
                    command3.ExecuteNonQuery();
                    command3.Dispose();

                    string BarrierId = "90f38534-8acc-11eb-a401-9d3cfc2ca371";
                    var command4 = new NpgsqlCommand { Connection = connection };
                    command4.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                        "WHERE id = '" + BarrierId + "';";
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
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "accepted/" + this.Id);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    if (content != "") { }

                    string BarrierId = "90f38534-8acc-11eb-a401-9d3cfc2ca371";

                    NpgsqlConnection connection = PostgreSQLConnection();
                    var command1 = new NpgsqlCommand { Connection = connection };
                    command1.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                        "WHERE id = '" + BarrierId + "';";
                    command1.ExecuteNonQuery();
                    command1.Dispose();
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
            var cs = "Host=127.0.0.1;Port=5432;Username=parking;Password=parking;Database=parking;";

            var con = new NpgsqlConnection(cs);
            con.Open();

            return con;
        }
    }
}
