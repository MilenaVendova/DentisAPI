using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace DentisAPI.Services
{
    public static class ConnectionManager
    {
        public static List<MyConnection> cnnList = new List<MyConnection>();
        private static IConfiguration? _Configuration;
        public static int CommandTimeout { get; set; }
        private static string? ConnectionString { get; set; }

        public static IConfiguration? Configuration
        {
            get => _Configuration;
            set
            {
                _Configuration = value;
                if (value != null)
                {
                    CommandTimeout = value.GetValue<int>("CommandTimeout:Default");
                    ConnectionString = value.GetConnectionString("DataConnection");
                }
            }
        }

        public static MyConnection? GetConnection(string User)
        {
            try
            {
                MyConnection? cnn;
                lock (cnnList)
                {
                    cnn = cnnList.Find(x => x.InUse == false);
                    if (cnn == null)
                    {
                        cnn = new MyConnection(ConnectionString, CommandTimeout);
                        cnnList.Add(cnn);
                    }
                    else
                    {
                        cnn.InUse = true;
                    }
                    cnn.user = User;
                }
                return cnn!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
    public class MyConnection
    {
        public SqlConnection cnn { get; set; }
        public string? user { get; set; }
        public bool InUse { get; set; }
        public DateTime LastUsed { get; set; }
        private SqlCommand _cmdExecuteAs { get; set; }
        public MyConnection(string? ConnectionString, int CommandTimeout)
        {
            cnn = new SqlConnection(ConnectionString);
            cnn.StateChange += new StateChangeEventHandler(cnn_StateChange);
            InUse = true;
            _cmdExecuteAs = new SqlCommand();
            _cmdExecuteAs.Connection = cnn;
            _cmdExecuteAs.CommandTimeout = CommandTimeout;
            _cmdExecuteAs.CommandText = "EXECUTE AS USER = @username;";
            _cmdExecuteAs.CommandType = CommandType.Text;
            _cmdExecuteAs.Parameters.Add("@username", SqlDbType.NVarChar, 50);
        }

        private void cnn_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                try
                {
                    _cmdExecuteAs.Parameters[0].Value = user;
                    _cmdExecuteAs.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public async void Release()
        {
            try
            {
                if (cnn.State == ConnectionState.Open)
                {
                    await cnn.CloseAsync();
                }
            }
            finally
            {
                InUse = false;
            }
        }
    }
}

