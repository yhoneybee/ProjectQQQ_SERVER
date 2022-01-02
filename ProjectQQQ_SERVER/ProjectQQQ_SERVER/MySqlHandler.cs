using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nettention.Proud;

namespace ProjectQQQ_SERVER
{
    //using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=root;Pwd=Rnfqjf2671!@#"))
    //{
    //    connection.Open();
    //    var cmd = new MySqlCommand("insert into users values (now(),now(),'kkulbeol',md5('123'))", connection);
    //    cmd.ExecuteNonQuery();
    //}
    public class MySqlHandler : IDisposable
    {
        public MySqlHandler(string server, string port, string database, string uid, string pwd)
        {
            Server = server;
            Port = port;
            Database = database;
            Uid = uid;
            Pwd = pwd;

            connection = new MySqlConnection($"{Server}{Port}{Database}{Uid}{Pwd}");
            connection.Open();
        }

        #region Connection Variables
        MySqlConnection connection;

        public string Server
        {
            get => server!;
            set => server = $"Server={value};";
        }
        private string? server;

        public string Port
        {
            get => port!;
            set => port = $"Port={value};";
        }
        private string? port;

        public string Database
        {
            get => database!;
            set => database = $"Database={value};";
        }
        private string? database;

        public string Uid
        {
            get => uid!;
            set => uid = $"Uid={value};";
        }
        private string? uid;

        public string Pwd
        {
            get => pwd!;
            set => pwd = $"Pwd={value};";
        }
        private string? pwd;
        #endregion

        public void InsertUser(string id, string pw, HostID hostID)
        {
            _Insert($"insert into users values (now(),now(),'{id}',md5('{pw}'),{Convert.ToInt32(hostID)})");
        }

        public void InsertRoom(string name, string pw, int id)
        {
            _Insert($"insert into rooms values ('{name}',md5('{pw}'),{id},0)");
        }

        public void InsertRoomUser(int roomId, string userId)
        {
            _Insert($"insert into roomusers values ({roomId},'{userId}')");
        }

        private void _Insert(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void _Update()
        {

        }

        private void _Delete()
        {

        }

        private void _Select()
        {

        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
