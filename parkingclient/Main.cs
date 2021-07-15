using System;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Media;
using System.Collections.Generic;
using System.Drawing.Printing;
using QRCoder;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;
using Npgsql;
using System.IO.Ports;
using parkingclient.Properties;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace parkingclient
{
    public partial class Main : Form
    {
        private delegate void InasertInDataGridDelegate(List<Events> events);
        private delegate void InsertInExitTextBoxDelegate(string text);

        public static string parkingId      = (string)Settings.Default["ParkingId"];
        public static string apiUrl         = (string)Settings.Default["ApiUrl"];
        public static string soundsFolder   = (string)Settings.Default["Sound"];
        public static string entryBarrier   = (string)Settings.Default["EntryBarrierId"];
        public static string exitBarrier    = (string)Settings.Default["ExitBarrierId"];
        public static string comPort        = (string)Settings.Default["ComPort"];

        NpgsqlConnection connection = null;
        private string currentDateTime = "";

        public Main()
        {
            InitializeComponent();
            var con = PostgreSQLConnection();
            connection = PostgreSQLConnection();
            Show();

            FillDropdowns();

            Thread checkNewEventsThread = new Thread(new ThreadStart(CheckEntryEvent))
            {
                IsBackground = true
            };
            checkNewEventsThread.Start();

            Thread syncLocalEventsThread = new Thread(new ThreadStart(SyncLocalEvents))
            {
                IsBackground = true
            };
            syncLocalEventsThread.Start();

            Thread syncAllEvents = new Thread(new ThreadStart(SyncNotLocalEvents))
            {
                IsBackground = true
            };
            syncAllEvents.Start();

            Thread getAllEventsThread = new Thread(new ThreadStart(UpdateDataGrid))
            {
                IsBackground = true
            };
            getAllEventsThread.Start();

            Thread getExitEventsThread = new Thread(new ThreadStart(CheckExitEvent))
            {
                IsBackground = true
            };
            getExitEventsThread.Start();

            Thread checkNeedToOpenExitBarrierThread = new Thread(new ThreadStart(NeedToOpenExitBarrier))
            {
                IsBackground = true
            };
            checkNeedToOpenExitBarrierThread.Start();

            Thread checkNeedToCloseExitBarrierThread = new Thread(new ThreadStart(NeedToCloseExitBarrier))
            {
                IsBackground = true
            };
            checkNeedToCloseExitBarrierThread.Start();

            this.ActiveControl = textBoxReader;
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
                    var cs = "Host=192.168.0.153;" +
                        "Port=8181;" +
                        "Username=parking;" +
                        "Password=parking;" +
                        "Database=parking;";

                    /*var cs = "Host=" + DatabaseHost + ";" +
                        "Port=" + DatabasePort + ";" +
                        "Username=" + DatabaseUser + ";" +
                        "Password=" + DatabasePass + ";" +
                        "Database=" + DatabaseName + ";";*/

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


        private void CheckEntryEvent()
        {
            while (true)
            {
                NpgsqlConnection con = PostgreSQLConnection();
                string sql = "SELECT id, carplate, islocalevent FROM parking_event WHERE isshown = false ORDER BY entry ASC";
                var command = new NpgsqlCommand(sql, con);

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    
                    try
                    {
                        string sum = "";
                        string tariff = "";
                        NpgsqlConnection connection = PostgreSQLConnection();

                        string query1 = "SELECT tariff FROM parking_event WHERE id = '" + reader.GetGuid(0).ToString() + "'";
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
                                                    "WHERE id = '" + reader.GetGuid(0).ToString() + "';";
                        command3.ExecuteNonQuery();
                        command3.Dispose();

                        if (!reader.GetBoolean(2))
                        {
                            try
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "accepted/" + reader.GetGuid(0).ToString());
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                                if (content != "") { }

                                /*NpgsqlConnection connection = PostgreSQLConnection();
                                var command1 = new NpgsqlCommand { Connection = connection };
                                command1.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                                    "WHERE id = '" + entryBarrier + "';";
                                command1.ExecuteNonQuery();
                                command1.Dispose();*/

                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }

                        var command4 = new NpgsqlCommand { Connection = connection };
                        command4.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                            "WHERE id = '" + entryBarrier + "';";
                        command4.ExecuteNonQuery();
                        command4.Dispose();

                        connection.Close();
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.ToString());
                    }
                    
                    //SoundPlayer simpleSound = new SoundPlayer(soundsFolder + "tada.wav");
                    //simpleSound.Play();
                    //var formPopup = new PopupIn(reader.GetGuid(0).ToString(), reader.GetString(1), reader.GetBoolean(2));
                    //formPopup.ShowDialog();
                }

                reader.Close();
                con.Close();

                Thread.Sleep(5000);
            }
        }

        private void CheckExitEvent()
        {
            while (true)
            {
                if (!String.IsNullOrEmpty(textBoxReader.Text))
                {
                    Thread.Sleep(1000);
                    NpgsqlConnection connection = PostgreSQLConnection();

                    string[] parametrs = textBoxReader.Text.Split('/');

                    if (parametrs[0] == "P")
                    {
                        string id = "";
                        string tariff = "";
                        string entry = "";
                        string exit = "";
                        double sum = 0;
                        string carplate = "";
                        bool isshown = false;
                        bool isaccepted = false;
                        bool isdenied = false;
                        bool islocalevent = false;
                        double perentry = 0;
                        double perhour = 0;
                        double perday = 0;
                        double permounth = 0;

                        string invoiceid = "";
                        string exitdatetime = "";
                        double totalsum = 0;

                        string query1 = "SELECT e.id, e.tariff, e.entry, e.exit, e.sum, e.carplate, e.isshown, e.isaccepted, e.isdenied, e.islocalevent, t.entry AS perentry, t.perhour AS perhour, t.perday AS perday, t.permounth AS permounth FROM parking_event e " +
                            "LEFT JOIN parking_tariff t ON e.tariff = t.id " +
                            "WHERE e.carplate = '" + parametrs[1] + "' AND e.isaccepted = true AND e.exit IS NULL " +
                            "GROUP BY e.id, t.entry, t.perhour, t.perday, t.permounth " +
                            "ORDER BY e.entry DESC LIMIT 1";
                        NpgsqlCommand command1 = new NpgsqlCommand(query1, connection);
                        NpgsqlDataReader reader1 = command1.ExecuteReader();

                        while (reader1.Read())
                        {
                            id = reader1.GetGuid(0).ToString();
                            tariff = reader1.GetGuid(1).ToString();
                            entry = reader1.GetDateTime(2).ToString();
                            exit = reader1.IsDBNull(3) ? "" : reader1.GetDateTime(3).ToString();
                            sum = reader1.GetDouble(4);
                            carplate = reader1.GetString(5);
                            isshown = reader1.GetBoolean(6);
                            isaccepted = reader1.GetBoolean(7);
                            isdenied = reader1.GetBoolean(8);
                            islocalevent = reader1.GetBoolean(9);
                            perentry = reader1.GetDouble(10);
                            perhour = reader1.GetDouble(11);
                            perday = reader1.GetDouble(12);
                            permounth = reader1.GetDouble(13);
                        }

                        reader1.Close();
                        command1.Dispose();


                        string query2 = "SELECT id FROM parking_invoice WHERE carplate = '" + parametrs[1] +
                            "' AND eventid = '" + id + "' AND ispayed = false;";
                        NpgsqlCommand command2 = new NpgsqlCommand(query2, connection);
                        NpgsqlDataReader reader2 = command2.ExecuteReader();

                        while (reader2.Read())
                        {
                            invoiceid = reader2.GetGuid(0).ToString();
                        }

                        reader2.Close();
                        command2.Dispose();

                        if (parametrs[0] == "P")
                        {
                            exitdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            exitdatetime = parametrs[2];
                        }

                        totalsum = GetTotalSum(entry, exitdatetime, perentry, perhour, perday, permounth);

                        var command3 = new NpgsqlCommand { Connection = connection };

                        if (invoiceid == "")
                        {
                            Guid obj = Guid.NewGuid();
                            string tsum = totalsum.ToString().Replace(',', '.');

                            command3.CommandText = "INSERT INTO parking_invoice (id, datetime, carplate, sum, eventid, islocalinvoice) VALUES('" +
                                            obj.ToString() + "', '" +
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                                            parametrs[1] + "', " +
                                            tsum + ", '" +
                                            id + "', true) RETURNING id;";
                            Object insinvoiceid = command3.ExecuteScalar();


                            command3.Dispose();

                            if (insinvoiceid.ToString() != "")
                            {
                                SoundPlayer simpleSound = new SoundPlayer(soundsFolder + "tada.wav");
                                simpleSound.Play();

                                var formPopup = new PopupOut(id, insinvoiceid.ToString(), parametrs[1], totalsum.ToString(), islocalevent);
                                formPopup.ShowDialog();
                            }
                        }
                        else
                        {
                            string tsum = totalsum.ToString().Replace(',', '.');

                            command3.CommandText = "UPDATE parking_invoice SET " +
                                        "datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                        "sum = " + tsum + " WHERE id = '" + invoiceid + "';";
                            command3.ExecuteNonQuery();
                            command3.Dispose();

                            SoundPlayer simpleSound = new SoundPlayer(soundsFolder + "tada.wav");
                            simpleSound.Play();

                            var formPopup = new PopupOut(id, invoiceid, parametrs[1], tsum, islocalevent);
                            formPopup.ShowDialog();
                        }
                    }
                    else if (parametrs[0] == "M" && parametrs[3] == "C")
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "createinvoice");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = "{\"type\":\"" + parametrs[0] + "\"," +
                                          "\"carplate\":\"" + parametrs[1] + "\"," +
                                          "\"datetime\":\"" + parametrs[2] + "\"," +
                                          "\"payingmethod\":\"" + parametrs[3] + "\"}";

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            string[] invoice = result.Split('/');

                            double doubleVal = ConvertToDouble(invoice[2]);
                            string stringVal = Convert.ToString(doubleVal);

                            SoundPlayer simpleSound = new SoundPlayer(soundsFolder + "tada.wav");
                            simpleSound.Play();
                            var formPopup = new PopupOut(invoice[1], invoice[0], parametrs[1], stringVal, false);
                            formPopup.ShowDialog();
                        }
                    }
                    else if (parametrs[0] == "M" && parametrs[3] == "O")
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "createinvoice");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = "{\"type\":\"" + parametrs[0] + "\"," +
                                          "\"carplate\":\"" + parametrs[1] + "\"," +
                                          "\"datetime\":\"" + parametrs[2] + "\"," +
                                          "\"payingmethod\":\"" + parametrs[3] + "\"}";

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            string[] invoice = result.Split('/');

                            double doubleVal = ConvertToDouble(invoice[2]);
                            string stringVal = Convert.ToString(doubleVal);

                            try
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "exitevent/" + invoice[0]);
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                                if (content != "") { }

                                NpgsqlConnection connection2 = PostgreSQLConnection();
                                var command = new NpgsqlCommand { Connection = connection2 };
                                command.CommandText = "UPDATE parking_barrier SET needtoopen = true WHERE id = '" + exitBarrier + "';";
                                command.ExecuteNonQuery();
                                command.Dispose();
                                connection2.Close();
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }
                    }

                    connection.Close();
                    InsertInExitTextBox("");

                    Thread.Sleep(4000);
                }
            }
        }

        private void SyncNotLocalEvents()
        {
            while (true)
            {
                if (IsConnectedToServer())
                {

                    //Syncing not local parking in events 
                    try
                    {
                        string content = "";
                        NpgsqlConnection con = PostgreSQLConnection();

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalinevents");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (content != "[]")
                        {
                            var allEvents = JsonConvert.DeserializeObject<List<Events>>(content);

                            try
                            {
                                foreach (var aevent in allEvents)
                                {
                                    if (aevent.Id != "")
                                    {
                                        string query = "SELECT id FROM parking_event WHERE id = '" + aevent.Id + "'";

                                        var command1 = new NpgsqlCommand { Connection = con };
                                        NpgsqlCommand command3 = new NpgsqlCommand(query, con);
                                        NpgsqlDataReader dr = command3.ExecuteReader();

                                        string sum = aevent.Sum.ToString().Replace(',', '.');

                                        if (dr.HasRows)
                                        {
                                            string exit = "";

                                            if (aevent.Exit == "")
                                            {
                                                exit = "exit = NULL, ";
                                            }
                                            else
                                            {
                                                exit = "exit = '" + aevent.Exit + "', ";
                                            }

                                            dr.Close();

                                            command1.CommandText = "UPDATE public.parking_event SET " +
                                                "tariff = '" + aevent.Tariff + "', " +
                                                "entry = '" + aevent.Entry + "', " +
                                                exit +
                                                "sum = " + sum + ", " +
                                                "carplate = '" + aevent.Carplate + "', " +
                                                "isshown = " + aevent.Isshown + ", " +
                                                "isaccepted = " + aevent.IsAccepted + ", " +
                                                "isdenied = " + aevent.IsDenied + ", " +
                                                "cartype = '" + aevent.CarType + "', " +
                                                "nationalitytype = '" + aevent.NationalityType + "', " +
                                                "parkingid = '" + aevent.ParkingId + "', " +
                                                "isclosed = " + aevent.IsClosed + " " +
                                                "WHERE id = '" + aevent.Id + "';";
                                            command1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            dr.Close();

                                            string exit = "";

                                            if (aevent.Exit == "")
                                            {
                                                exit = "NULL, ";
                                            }
                                            else
                                            {
                                                exit = "'" + aevent.Exit + "', ";
                                            }

                                            command1.CommandText = "INSERT INTO parking_event(id, tariff, entry, exit, sum, carplate, isshown, isaccepted, isdenied, cartype, nationalitytype, parkingid, isclosed, issynced) VALUES('" +
                                                    aevent.Id + "', '" +
                                                    aevent.Tariff + "', '" +
                                                    aevent.Entry + "', " +
                                                    exit +
                                                    sum + ", '" +
                                                    aevent.Carplate + "', " +
                                                    aevent.Isshown + ", " +
                                                    aevent.IsAccepted + ", " +
                                                    aevent.IsDenied + ", '" +
                                                    aevent.CarType + "', '" +
                                                    aevent.NationalityType + "', '" +
                                                    aevent.ParkingId + "', " +
                                                    aevent.IsClosed + ", True)";
                                            command1.ExecuteNonQuery();
                                        }
                                        command1.Dispose();
                                        command3.Dispose();

                                        try
                                        {
                                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalin/" + aevent.Id);
                                            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                            var content2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();

                                            if (content2 != "") { }
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }
                        con.Close();
                    }
                    catch (Exception msg1)
                    {
                        Console.WriteLine(msg1.ToString());
                    }

                    //Syncing not local parking out events 
                    try
                    {
                        string content = "";
                        NpgsqlConnection con = PostgreSQLConnection();

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocaloutevents");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (content != "[]")
                        {
                            var allEvents = JsonConvert.DeserializeObject<List<Events>>(content);

                            try
                            {
                                foreach (var aevent in allEvents)
                                {
                                    if (aevent.Id != "")
                                    {
                                        string query = "SELECT id FROM parking_event WHERE id = '" + aevent.Id + "'";

                                        var command1 = new NpgsqlCommand { Connection = con };
                                        NpgsqlCommand command3 = new NpgsqlCommand(query, con);
                                        NpgsqlDataReader dr = command3.ExecuteReader();

                                        string sum = aevent.Sum.ToString().Replace(',', '.');

                                        if (dr.HasRows)
                                        {
                                            string exit = "";

                                            if (aevent.Exit == "")
                                            {
                                                exit = "exit = NULL, ";
                                            }
                                            else
                                            {
                                                exit = "exit = '" + aevent.Exit + "', ";
                                            }

                                            dr.Close();

                                            command1.CommandText = "UPDATE public.parking_event SET " +
                                                "tariff = '" + aevent.Tariff + "', " +
                                                "entry = '" + aevent.Entry + "', " +
                                                exit +
                                                "sum = " + sum + ", " +
                                                "carplate = '" + aevent.Carplate + "', " +
                                                "isshown = " + aevent.Isshown + ", " +
                                                "isaccepted = " + aevent.IsAccepted + ", " +
                                                "isdenied = " + aevent.IsDenied + ", " +
                                                "cartype = '" + aevent.CarType + "', " +
                                                "nationalitytype = '" + aevent.NationalityType + "', " +
                                                "parkingid = '" + aevent.ParkingId + "', " +
                                                "isclosed = " + aevent.IsClosed + " " +
                                                "WHERE id = '" + aevent.Id + "';";
                                            command1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            dr.Close();

                                            string exit = "";

                                            if (aevent.Exit == "")
                                            {
                                                exit = "NULL, ";
                                            }
                                            else
                                            {
                                                exit = "'" + aevent.Exit + "', ";
                                            }

                                            command1.CommandText = "INSERT INTO parking_event(id, tariff, entry, exit, sum, carplate, isshown, isaccepted, isdenied, cartype, nationalitytype, parkingid, isclosed, issynced) VALUES('" +
                                                    aevent.Id + "', '" +
                                                    aevent.Tariff + "', '" +
                                                    aevent.Entry + "', " +
                                                    exit +
                                                    sum + ", '" +
                                                    aevent.Carplate + "', " +
                                                    aevent.Isshown + ", " +
                                                    aevent.IsAccepted + ", " +
                                                    aevent.IsDenied + ", '" +
                                                    aevent.CarType + "', '" +
                                                    aevent.NationalityType + "', '" +
                                                    aevent.ParkingId + "', " +
                                                    aevent.IsClosed + ", True)";
                                            command1.ExecuteNonQuery();
                                        }
                                        command1.Dispose();
                                        command3.Dispose();

                                        try
                                        {
                                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalout/" + aevent.Id);
                                            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                            var content2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();

                                            if (content2 != "") { }
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }
                        con.Close();
                    }
                    catch (Exception msg1)
                    {
                        Console.WriteLine(msg1.ToString());
                    }

                    //Syncing not local created invoices 
                    try
                    {
                        string content = "";
                        NpgsqlConnection con = PostgreSQLConnection();

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalcreatedinvoices");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (content != "[]")
                        {
                            var allInvoices = JsonConvert.DeserializeObject<List<Invoices>>(content);

                            try
                            {
                                foreach (var ainvoice in allInvoices)
                                {
                                    if (ainvoice.Id != "")
                                    {
                                        string query = "SELECT id FROM parking_invoice WHERE id = '" + ainvoice.Id + "'";

                                        var command1 = new NpgsqlCommand { Connection = con };
                                        NpgsqlCommand command3 = new NpgsqlCommand(query, con);
                                        NpgsqlDataReader dr = command3.ExecuteReader();

                                        string sum = ainvoice.Sum.ToString().Replace(',', '.');

                                        if (dr.HasRows)
                                        {
                                            dr.Close();

                                            command1.CommandText = "UPDATE parking_invoice SET " +
                                                "datetime = '" + ainvoice.Datetime + "', " +
                                                "carplate = '" + ainvoice.Carplate + "', " +
                                                "sum = " + sum + ", " +
                                                "eventid = '" + ainvoice.Eventid + "', " +
                                                "ispayed = " + ainvoice.Ispayed + ", " +
                                                "islocalinvoice = " + ainvoice.Islocalinvoice + " " +
                                                "WHERE id = '" + ainvoice.Id + "';";
                                            command1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            dr.Close();

                                            command1.CommandText = "INSERT INTO parking_invoice (id, datetime, carplate, sum, eventid, ispayed, islocalinvoice) VALUES('" +
                                                    ainvoice.Id + "', '" +
                                                    ainvoice.Datetime + "', '" +
                                                    ainvoice.Carplate + "', " +
                                                    sum + ", '" +
                                                    ainvoice.Eventid + "', " +
                                                    ainvoice.Ispayed + ", " +
                                                    ainvoice.Islocalinvoice + ")";
                                            command1.ExecuteNonQuery();
                                        }
                                        command1.Dispose();
                                        command3.Dispose();

                                        try
                                        {
                                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalcreatedinvoice/" + ainvoice.Id);
                                            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                            var content2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();

                                            if (content2 != "") { }
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }
                        con.Close();
                    }
                    catch (Exception msg1)
                    {
                        Console.WriteLine(msg1.ToString());
                    }

                    //Syncing not local payed invoices 
                    try
                    {
                        string content = "";
                        NpgsqlConnection con = PostgreSQLConnection();

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalpayedinvoices");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        content = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (content != "[]")
                        {
                            var allInvoices = JsonConvert.DeserializeObject<List<Invoices>>(content);

                            try
                            {
                                foreach (var ainvoice in allInvoices)
                                {
                                    if (ainvoice.Id != "")
                                    {
                                        string query = "SELECT id FROM parking_invoice WHERE id = '" + ainvoice.Id + "'";

                                        var command1 = new NpgsqlCommand { Connection = con };
                                        NpgsqlCommand command3 = new NpgsqlCommand(query, con);
                                        NpgsqlDataReader dr = command3.ExecuteReader();

                                        string sum = ainvoice.Sum.ToString().Replace(',', '.');

                                        if (dr.HasRows)
                                        {
                                            dr.Close();

                                            command1.CommandText = "UPDATE parking_invoice SET " +
                                                "datetime = '" + ainvoice.Datetime + "', " +
                                                "carplate = '" + ainvoice.Carplate + "', " +
                                                "sum = " + sum + ", " +
                                                "eventid = '" + ainvoice.Eventid + "', " +
                                                "ispayed = " + ainvoice.Ispayed + ", " +
                                                "islocalinvoice = " + ainvoice.Islocalinvoice + " " +
                                                "WHERE id = '" + ainvoice.Id + "';";
                                            command1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            dr.Close();

                                            command1.CommandText = "INSERT INTO parking_invoice (id, datetime, carplate, sum, eventid, ispayed, islocalinvoice) VALUES('" +
                                                    ainvoice.Id + "', '" +
                                                    ainvoice.Datetime + "', '" +
                                                    ainvoice.Carplate + "', " +
                                                    sum + ", '" +
                                                    ainvoice.Eventid + "', " +
                                                    ainvoice.Ispayed + ", " +
                                                    ainvoice.Islocalinvoice + ")";
                                            command1.ExecuteNonQuery();
                                        }
                                        command1.Dispose();
                                        command3.Dispose();

                                        try
                                        {
                                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(@"" + apiUrl + "syncnotlocalpayedinvoice/" + ainvoice.Id);
                                            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                            var content2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();

                                            if (content2 != "") { }
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine(msg.ToString());
                            }
                        }
                        con.Close();
                    }
                    catch (Exception msg1)
                    {
                        Console.WriteLine(msg1.ToString());
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private void SyncLocalEvents()
        {
            while (true)
            {
                if (IsConnectedToServer())
                {
                    NpgsqlConnection connection = PostgreSQLConnection();

                    //Syncing local parking in events 
                    try
                    {
                        string sql = "SELECT id, tariff, entry, exit, sum, carplate, isshown, isaccepted, isdenied, cartype, nationalitytype, parkingid, isclosed " +
                            "FROM parking_event " +
                            "WHERE islocalevent = true AND issyncedin = false AND NOT sum = 0;";
                        var command1 = new NpgsqlCommand(sql, connection);
                        NpgsqlDataReader reader1 = command1.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                string id = reader1.GetGuid(0).ToString();

                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "synclocalinevents");
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    string exit = reader1.IsDBNull(3) ? "" : reader1.GetDateTime(3).ToString("yyyy-MM-dd HH:mm:ss");
                                    string sum = reader1.GetDouble(4).ToString().Replace(',', '.');
                                    string json = "{\"id\":\"" + id + "\"," +
                                                  "\"tariff\":\"" + reader1.GetGuid(1).ToString() + "\"," +
                                                  "\"entry\":\"" + reader1.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                  "\"exit\":\"" + exit + "\"," +
                                                  "\"sum\":" + sum + "," +
                                                  "\"carplate\":\"" + reader1.GetString(5).ToString() + "\"," +
                                                  "\"isshown\":\"" + reader1.GetBoolean(6).ToString() + "\"," +
                                                  "\"isaccepted\":\"" + reader1.GetBoolean(7).ToString() + "\"," +
                                                  "\"isdenied\":\"" + reader1.GetBoolean(8).ToString() + "\"," +
                                                  "\"cartype\":\"" + reader1.GetGuid(9).ToString() + "\"," +
                                                  "\"nationalitytype\":\"" + reader1.GetGuid(10).ToString() + "\"," +
                                                  "\"parkingid\":\"" + reader1.GetGuid(11).ToString() + "\"," +
                                                  "\"isclosed\":\"" + reader1.GetBoolean(12).ToString() + "\"}";

                                    streamWriter.Write(json);
                                }

                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();

                                    if (result == "OK")
                                    {
                                        try
                                        {
                                            var con = PostgreSQLConnection();
                                            var command2 = new NpgsqlCommand { Connection = con };
                                            command2.CommandText = "UPDATE parking_event SET issyncedin = true WHERE id = '" + id + "';";
                                            command2.ExecuteNonQuery();
                                            command2.Dispose();
                                            con.Close();
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        reader1.Close();
                        command1.Dispose();
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.ToString());
                    }

                    //Syncing local parking out events 
                    try
                    {
                        string sql = "SELECT id, tariff, entry, exit, sum, carplate, isshown, isaccepted, isdenied, cartype, nationalitytype, parkingid, isclosed " +
                            "FROM parking_event " +
                            "WHERE islocalevent = true AND issyncedin = true AND issyncedout = false AND exit IS NOT NULL;";
                        var command3 = new NpgsqlCommand(sql, connection);
                        NpgsqlDataReader reader3 = command3.ExecuteReader();

                        if (reader3.HasRows)
                        {
                            while (reader3.Read())
                            {
                                string id = reader3.GetGuid(0).ToString();

                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "synclocaloutevents");
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    string exit = reader3.IsDBNull(3) ? "" : reader3.GetDateTime(3).ToString("yyyy-MM-dd HH:mm:ss");
                                    string sum = reader3.GetDouble(4).ToString().Replace(',', '.');
                                    string json = "{\"id\":\"" + id + "\"," +
                                                  "\"tariff\":\"" + reader3.GetGuid(1).ToString() + "\"," +
                                                  "\"entry\":\"" + reader3.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                  "\"exit\":\"" + exit + "\"," +
                                                  "\"sum\":" + sum + "," +
                                                  "\"carplate\":\"" + reader3.GetString(5).ToString() + "\"," +
                                                  "\"isshown\":\"" + reader3.GetBoolean(6).ToString() + "\"," +
                                                  "\"isaccepted\":\"" + reader3.GetBoolean(7).ToString() + "\"," +
                                                  "\"isdenied\":\"" + reader3.GetBoolean(8).ToString() + "\"," +
                                                  "\"cartype\":\"" + reader3.GetGuid(9).ToString() + "\"," +
                                                  "\"nationalitytype\":\"" + reader3.GetGuid(10).ToString() + "\"," +
                                                  "\"parkingid\":\"" + reader3.GetGuid(11).ToString() + "\"," +
                                                  "\"isclosed\":\"" + reader3.GetBoolean(12).ToString() + "\"}";

                                    streamWriter.Write(json);
                                }

                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();

                                    if (result == "OK")
                                    {
                                        try
                                        {
                                            var con2 = PostgreSQLConnection();
                                            var command4 = new NpgsqlCommand { Connection = con2 };
                                            command4.CommandText = "UPDATE parking_event SET issyncedout = true WHERE id = '" + id + "';";
                                            command4.ExecuteNonQuery();
                                            command4.Dispose();
                                            con2.Close();
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        reader3.Close();
                        command3.Dispose();
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.ToString());
                    }

                    //Syncing local created invoices 
                    try
                    {
                        string sql5 = "SELECT id, datetime, carplate, sum, eventid, ispayed " +
                            "FROM parking_invoice " +
                            "WHERE islocalinvoice = true AND issyncedcreation = false;";
                        var command5 = new NpgsqlCommand(sql5, connection);
                        NpgsqlDataReader reader5 = command5.ExecuteReader();

                        if (reader5.HasRows)
                        {
                            while (reader5.Read())
                            {
                                string id = reader5.GetGuid(0).ToString();

                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "synclocalcreatedinvoices");
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    string sum = reader5.GetDouble(3).ToString().Replace(',', '.');
                                    string json = "{\"id\":\"" + id + "\"," +
                                                  "\"datetime\":\"" + reader5.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                  "\"carplate\":\"" + reader5.GetString(2).ToString() + "\"," +
                                                  "\"sum\":" + sum + "," +
                                                  "\"eventid\":\"" + reader5.GetGuid(4).ToString() + "\"," +
                                                  "\"ispayed\":\"" + reader5.GetBoolean(5).ToString() + "\"}";

                                    streamWriter.Write(json);
                                }

                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();

                                    if (result == "OK")
                                    {
                                        try
                                        {
                                            var con3 = PostgreSQLConnection();
                                            var command6 = new NpgsqlCommand { Connection = con3 };
                                            command6.CommandText = "UPDATE parking_invoice SET issyncedcreation = true WHERE id = '" + id + "';";
                                            command6.ExecuteNonQuery();
                                            command6.Dispose();
                                            con3.Close();
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        reader5.Close();
                        reader5.Dispose();
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.ToString());
                    }

                    //Syncing local payed invoices 
                    try
                    {
                        string sql7 = "SELECT id, datetime, carplate, sum, eventid, ispayed " +
                            "FROM parking_invoice " +
                            "WHERE islocalinvoice = true AND ispayed = true AND issyncedcreation = true AND issyncedpayed = false;";
                        var command7 = new NpgsqlCommand(sql7, connection);
                        NpgsqlDataReader reader7 = command7.ExecuteReader();

                        if (reader7.HasRows)
                        {
                            while (reader7.Read())
                            {
                                string id = reader7.GetGuid(0).ToString();

                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "synclocalpayedinvoices");
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    string sum = reader7.GetDouble(3).ToString().Replace(',', '.');
                                    string json = "{\"id\":\"" + id + "\"," +
                                                  "\"datetime\":\"" + reader7.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                  "\"carplate\":\"" + reader7.GetString(2).ToString() + "\"," +
                                                  "\"sum\":" + sum + "," +
                                                  "\"eventid\":\"" + reader7.GetGuid(4).ToString() + "\"," +
                                                  "\"ispayed\":\"" + reader7.GetBoolean(5).ToString() + "\"}";

                                    streamWriter.Write(json);
                                }

                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();

                                    if (result == "OK")
                                    {
                                        try
                                        {
                                            var con4 = PostgreSQLConnection();
                                            var command8 = new NpgsqlCommand { Connection = con4 };
                                            command8.CommandText = "UPDATE parking_invoice SET issyncedpayed = true WHERE id = '" + id + "';";
                                            command8.ExecuteNonQuery();
                                            command8.Dispose();
                                            con4.Close();
                                        }
                                        catch (Exception msg)
                                        {
                                            Console.WriteLine(msg.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        reader7.Close();
                        reader7.Dispose();
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg.ToString());
                    }

                    connection.Close();
                }

                Thread.Sleep(5000);
            }
        }


        private void UpdateDataGrid()
        {
            while (true)
            {
                NpgsqlConnection con = PostgreSQLConnection();

                try {
                    List<Events> allEvents2 = new List<Events>();

                    string sql = "SELECT id, tariff, entry, exit, sum, carplate, isshown, isaccepted, isdenied, cartype, nationalitytype, parkingid, isclosed FROM parking_event ORDER BY entry DESC";
                    var command2 = new NpgsqlCommand(sql, con);

                    NpgsqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read())
                    {
                        allEvents2.Add(
                            new Events(
                                reader2.GetGuid(0).ToString(),
                                reader2.GetGuid(1).ToString(),
                                reader2.GetDateTime(2).ToString(),
                                reader2.IsDBNull(3) ? "" : reader2.GetDateTime(3).ToString(),
                                reader2.IsDBNull(4) ? 0 : reader2.GetDouble(4),
                                reader2.GetString(5),
                                reader2.GetBoolean(6),
                                reader2.GetBoolean(7),
                                reader2.GetBoolean(8),
                                reader2.GetGuid(9).ToString(),
                                reader2.GetGuid(10).ToString(),
                                reader2.GetGuid(11).ToString(),
                                reader2.GetBoolean(12)
                            )
                        );
                    }

                    InasertInDataGrid(allEvents2);
                    reader2.Close();
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.ToString());
                    throw;
                }

                con.Close();
                Thread.Sleep(5000);
            }
        }

        private void InasertInDataGrid(List<Events> events)
        {
            if (allEventsDataGridView.InvokeRequired)
            {
                var d = new InasertInDataGridDelegate(InasertInDataGrid);
                allEventsDataGridView.Invoke(d, new object[] { events });
            }
            else
            {
                allEventsDataGridView.DataSource = events;
            }
        }


        private void ButtonPrint_Click(System.Object sender, EventArgs e)
        {
            Guid obj = Guid.NewGuid();
            labelCarplate.Text = obj.ToString();

            Type selectedCarType = (Type)comboBoxCarType.SelectedValue;
            Type selectedNationalityType = (Type)comboBoxNationalityType.SelectedValue;
            //string ParkingId = (string)Settings.Default["ParkingId"];

            try
            {
                NpgsqlConnection conn = PostgreSQLConnection();
                bool isopen = false;
                string tariff = "";
                currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query1 = "SELECT isopen FROM parking_lot WHERE id = '" + parkingId + "'";

                NpgsqlCommand command1 = new NpgsqlCommand(query1, conn);
                NpgsqlDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read()) { isopen = reader1.GetBoolean(0); }

                reader1.Close();
                command1.Dispose();


                string query2 = "SELECT id FROM parking_tariff WHERE nationalitytype = '" + selectedNationalityType.Value + "' AND cartype = '" + selectedCarType.Value + "' AND isopen = " + isopen + ";";

                NpgsqlCommand command2 = new NpgsqlCommand(query2, conn);
                NpgsqlDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    tariff = reader2.GetGuid(0).ToString();
                }
                reader2.Close();
                command2.Dispose();

                var command3 = new NpgsqlCommand { Connection = conn };

                command3.CommandText = "INSERT INTO parking_event(id, tariff, entry, carplate, cartype, nationalitytype, parkingid, islocalevent) VALUES('" +
                    labelCarplate.Text + "', '" +
                    tariff + "', '" +
                    currentDateTime + "', '" +
                    labelCarplate.Text + "', '" +
                    selectedCarType.Value + "', '" +
                    selectedNationalityType.Value + "', '" +
                    parkingId + "', " +
                    "true)";
                command3.ExecuteNonQuery();

                command3.Dispose();

                try
                {
                    printDocumentQRCode.PrintPage += new PrintPageEventHandler(QRDocument_PrintPage);
                    printDocumentQRCode.Print();

                    labelCarplate.Text = "";
                    pictureBox1.Image = null;
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.ToString());
                    throw;
                }
                conn.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
            this.ActiveControl = textBoxReader;
        }

        private void QRDocument_PrintPage(System.Object sender, PrintPageEventArgs e)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("P/" + labelCarplate.Text + "/" + currentDateTime + "/C", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(4);

            pictureBox1.Image = qrCodeImage;

            pictureBox1.DrawToBitmap(qrCodeImage, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            e.Graphics.DrawImage(qrCodeImage, 0, 0);
        }



        private double GetTotalSum(string entry, string exitdatetime, double perentry, double perhour, double perday, double permounth)
        {
            double totalsum = perentry;

            DateTime startTime = DateTime.Parse(entry);
            DateTime endTime = DateTime.Parse(exitdatetime);

            TimeSpan span = endTime.Subtract(startTime);

            double duration = Math.Ceiling((double)span.Hours);

            if (duration > 7200) {
                var mod = duration / 7200;
                totalsum += permounth * mod;
                duration = duration - (7200 * mod);
            }

            if (duration > 24)
            {
                var mod = duration / 24;
                totalsum += perday * mod;
                duration = duration - (24 * mod);
            }

            if (duration > 0)
            {
                totalsum += perhour * duration;
            }

            return totalsum;
        }

        private void InsertInExitTextBox(string text)
        {
            if (textBoxReader.InvokeRequired)
            {
                var d = new InsertInExitTextBoxDelegate(InsertInExitTextBox);
                textBoxReader.Invoke(d, new object[] { text });
            }
            else
            {
                textBoxReader.Text = text;
            }
        }

        private void FillDropdowns() {
            List<Type> carTypes = new List<Type>();
            List<Type> nationalityTypes = new List<Type>();

            carTypes.Add(new Type() { Value = "a8766eff-2f0e-4cb4-ab0c-5b4fd166e945", Text = "Yenil awtoulag" });
            carTypes.Add(new Type() { Value = "5a91f0dd-3675-43c3-ac41-ee6b5566d35b", Text = "Yuk, mikrawtobus 3 ton cenli" });
            carTypes.Add(new Type() { Value = "2c2b1109-c2f4-4f6d-8b79-18ec8096ef16", Text = "Yuk, awtobus 3 tonnadan agyr" });
            carTypes.Add(new Type() { Value = "d155046f-396a-47aa-91dd-fdfbd5fb5ed4", Text = "Tirkegli, yarym trk, uzyn awtobus" });
            carTypes.Add(new Type() { Value = "4494de27-639f-40a1-8de9-2df38ca2af28", Text = "Motosikl, moroller, motokolyaska" });
            carTypes.Add(new Type() { Value = "53652c41-2017-4c0b-a29c-12fbfa6912fe", Text = "Tirkegli motosikl" });
            carTypes.Add(new Type() { Value = "9a3e82ee-1adb-4082-b1f1-405fa6696aae", Text = "Moped, welosiped" });
            carTypes.Add(new Type() { Value = "4659dd31-bcbd-4f08-ac6b-6c18129aae54", Text = "Yenil awto satlyk ucin" });

            nationalityTypes.Add(new Type() { Value = "8438f408-bfa8-4408-92c7-b5292471d8c5", Text = "Turkmenistanyn rayat" });
            nationalityTypes.Add(new Type() { Value = "a1cf2b5e-e2fe-4222-adbb-85d7eb0f4b91", Text = "Dasary yurt rayat" });

            comboBoxCarType.DataSource = carTypes;
            comboBoxCarType.DisplayMember = "Text";

            comboBoxNationalityType.DataSource = nationalityTypes;
            comboBoxNationalityType.DisplayMember = "Text";
        }

        private double ConvertToDouble(string s)
        {
            char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            double result = 0;
            try
            {
                if (s != null)
                    if (!s.Contains(","))
                        result = double.Parse(s, CultureInfo.InvariantCulture);
                    else
                        result = Convert.ToDouble(s.Replace(".", systemSeparator.ToString()).Replace(",", systemSeparator.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                try
                {
                    result = Convert.ToDouble(s);
                }
                catch
                {
                    try
                    {
                        result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
                    }
                    catch
                    {
                        throw new Exception("Wrong string-to-double format");
                    }
                }
            }
            return result;
        }

        private bool IsConnectedToServer()
        {
            try
            {
                Regex reg = new Regex(@"([0-9]{1,3}[\.]){3}[0-9]{1,3}");
                Match result = reg.Match(apiUrl);

                Ping ping = new Ping();
                PingReply reply = ping.Send(result.ToString(), 3000);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.ToString());
            }

            return false;
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            var formPopup = new AppSettings();
            formPopup.ShowDialog();
            this.ActiveControl = textBoxReader;
        }

        
        ///Entry Barrier

        private void ButtonOpenEntryBarrier_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection connection = PostgreSQLConnection();

                var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                    "WHERE id = '" + entryBarrier + "' AND needtoclose = false;";
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
            this.ActiveControl = textBoxReader;
        }

        private void ButtonCloseEntryBarrier_Click(object sender, EventArgs e)
        {
            try
            {
                string BarrierId = "90f38534-8acc-11eb-a401-9d3cfc2ca371";

                NpgsqlConnection connection = PostgreSQLConnection();

                var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "UPDATE parking_barrier SET needtoclose = true " +
                                    "WHERE id = '" + BarrierId + "' AND needtoopen = false;";
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
            this.ActiveControl = textBoxReader;
        }
        

        ///Exit Barrier
        
        private void OpenBarrier()
        {
            try
            {
                SerialPort port = new SerialPort
                {
                    PortName = comPort,
                    BaudRate = Convert.ToInt32(9600),
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                if (port.IsOpen == true)
                    port.Close();

                if (port.IsOpen == false)
                {
                    port.Open();
                    port.ReadTimeout = 3000;
                    port.Write("1");
                    port.Close();
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
        }

        private void CloseBarrier()
        {
            try
            {
                SerialPort port = new SerialPort
                {
                    PortName = comPort,
                    BaudRate = Convert.ToInt32(9600),
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                if (port.IsOpen == true)
                    port.Close();

                if (port.IsOpen == false)
                {
                    port.Open();
                    port.ReadTimeout = 3000;
                    port.Write("2");
                    port.Close();
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
        }

        private void NeedToOpenExitBarrier()
        {
            while (true)
            {
                if (exitBarrier != "")
                {
                    bool needtoopen = false;

                    NpgsqlConnection connection = PostgreSQLConnection();
                    string sql = "SELECT needtoopen FROM parking_barrier WHERE id = '" + exitBarrier + "';";
                    var command = new NpgsqlCommand(sql, connection);
                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        needtoopen = reader.GetBoolean(0);
                    }

                    reader.Close();
                    command.Dispose();

                    if (needtoopen == true)
                    {
                        //OpenBarrier();

                        var command2 = new NpgsqlCommand { Connection = connection };
                        command2.CommandText = "UPDATE parking_barrier SET needtoopen = false " +
                                            "WHERE id = '" + exitBarrier + "';";
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                    }
                    connection.Close();
                }
                Thread.Sleep(3000);
            }
        }

        private void NeedToCloseExitBarrier()
        {
            while (true)
            {
                if (exitBarrier != "")
                {
                    bool needtoclose = false;

                    NpgsqlConnection connection = PostgreSQLConnection();
                    string sql = "SELECT needtoclose FROM parking_barrier WHERE id = '" + exitBarrier + "';";
                    var command = new NpgsqlCommand(sql, connection);
                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        needtoclose = reader.GetBoolean(0);
                    }

                    reader.Close();
                    command.Dispose();

                    if (needtoclose == true)
                    {
                        //CloseBarrier();

                        var command2 = new NpgsqlCommand { Connection = connection };
                        command2.CommandText = "UPDATE parking_barrier SET needtoclose = false " +
                                            "WHERE id = '" + exitBarrier + "';";
                        command2.ExecuteNonQuery();
                        command2.Dispose();
                    }
                    connection.Close();


                }
                Thread.Sleep(3000);
            }
        }

        private void ButtonOpenExitBarrier_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection connection = PostgreSQLConnection();

                var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "UPDATE parking_barrier SET needtoopen = true " +
                                    "WHERE id = '" + exitBarrier + "' AND needtoclose = false;";
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
            this.ActiveControl = textBoxReader;
        }
        
        private void ButtonCloseExitBarrier_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection connection = PostgreSQLConnection();

                var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "UPDATE parking_barrier SET needtoclose = true " +
                                    "WHERE id = '" + exitBarrier + "' AND needtoopen = false;";
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
                throw;
            }
            this.ActiveControl = textBoxReader;
        }
    }

    public class NewEvent
    {
        private string id;
        private string tariff;
        private string entry;
        private double sum;
        private string carplate;
        private bool isshown;

        public NewEvent(string id, string tariff, string entry, double sum, string carplate, bool isshown)
        {
            this.id = id;
            this.tariff = tariff;
            this.entry = entry;
            this.sum = sum;
            this.carplate = carplate;
            this.isshown = isshown;
        }

        public string Id {
            get { return id; }
            set { id = value; }
        }
        public string Tariff {
            get { return tariff; }
            set { tariff = value; }
        }
        public string Entry {
            get { return entry; }
            set { entry = value; }
        }
        public double Sum {
            get { return sum; }
            set { sum = value; }
        }
        public string Carplate {
            get { return carplate; }
            set { carplate = value; }
        }
        public bool Isshown {
            get { return isshown; }
            set { isshown = value; }
        }
    }

    public class Events
    {
        private string id;
        private string tariff;
        private string entry;
        private string exit;
        private double sum;
        private string carplate;
        private bool isshown;
        private bool isaccepted;
        private bool isdenied;
        private string cartype;
        private string nationalitytype;
        private string parkingid;
        private bool isclosed;

        public Events(string id, string tariff, string entry, string exit, double sum, string carplate, bool isshown, bool isaccepted, bool isdenied, string cartype, string nationalitytype, string parkingid, bool isclosed)
        {
            this.id = id;
            this.tariff = tariff;
            this.entry = entry;
            this.exit = exit;
            this.sum = sum;
            this.carplate = carplate;
            this.isshown = isshown;
            this.isaccepted = isaccepted;
            this.isdenied = isdenied;
            this.cartype = cartype;
            this.nationalitytype = nationalitytype;
            this.parkingid = parkingid;
            this.isclosed = isclosed;
        }

        public string Id {
            get { return id; }
            set { id = value; }
        }
        public string Tariff {
            get { return tariff; }
            set { tariff = value; }
        }
        public string Entry {
            get { return entry; }
            set { entry = value; }
        }   
        public string Exit {
            get { return exit; }
            set { exit = value; }
        }
        public double Sum {
            get { return sum; }
            set { sum = value; }
        }
        public string Carplate {
            get { return carplate; }
            set { carplate = value; }
        }
        public bool Isshown {
            get { return isshown; }
            set { isshown = value; }
        }
        public bool IsAccepted {
            get { return isaccepted; }
            set { isaccepted = value; }
        }
        public bool IsDenied {
            get { return isdenied; }
            set { isdenied = value; }
        }
        public string CarType {
            get { return cartype; }
            set { cartype = value; }
        }
        public string NationalityType {
            get { return nationalitytype; }
            set { nationalitytype = value; }
        }
        public string ParkingId {
            get { return parkingid; }
            set { parkingid = value; }
        }
        public bool IsClosed
        {
            get { return isclosed; }
            set { isclosed = value; }
        }
    }

    public class Invoices
    {
        private string id;
        private string datetime;
        private string carplate;
        private double sum;
        private string eventid;
        private bool ispayed;
        private bool islocalinvoice;

        public Invoices(string id, string datetime, string carplate, double sum, string eventid, bool ispayed, bool islocalinvoice)
        {
            this.id = id;
            this.datetime = datetime;
            this.carplate = carplate;
            this.sum = sum;
            this.eventid = eventid;
            this.ispayed = ispayed;
            this.islocalinvoice = islocalinvoice;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }
        public string Carplate
        {
            get { return carplate; }
            set { carplate = value; }
        }
        public double Sum
        {
            get { return sum; }
            set { sum = value; }
        }
        public string Eventid
        {
            get { return eventid; }
            set { eventid = value; }
        }
        public bool Ispayed
        {
            get { return ispayed; }
            set { ispayed = value; }
        }
        public bool Islocalinvoice
        {
            get { return islocalinvoice; }
            set { islocalinvoice = value; }
        }
    }

    class Type
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
