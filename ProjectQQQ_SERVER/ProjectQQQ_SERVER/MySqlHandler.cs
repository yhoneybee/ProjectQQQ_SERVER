using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nettention.Proud;

namespace ProjectQQQ_SERVER
{
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
            _Insert($"insert into users values (now(), now(), '{id}', md5('{pw}'), {Convert.ToInt32(hostID)})");
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
                if (cmd.ExecuteNonQuery() != 1)
                    Console.WriteLine("***failed to insert data.***");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void UpdateUser(string id, HostID hostID)
        {
            _Update($"update users set LostLoginDate = now(), HostID = {Convert.ToInt32(hostID)} where ID = '{id}'");
        }

        public void UpdateRooms(string id, int clientCount)
        {
            _Update($"update rooms set ClientCount = {clientCount} where ID = '{id}'");
        }

        private void _Update(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, connection);
                if (cmd.ExecuteNonQuery() != 1)
                    Console.WriteLine("***failed to update data.***");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteUser(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            _Delete($"delete from users {where}");
        }

        public void DeleteRoom(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            _Delete($"delete from rooms {where}");
        }

        public void DeleteRoomUser(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            _Delete($"delete from roomusers {where}");
        }

        private void _Delete(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, connection);
                if (cmd.ExecuteNonQuery() != 1)
                    Console.WriteLine("***failed to delete data.***");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<User> SelectUser(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            var table = _Select($"select * from users {where}");

            List<User> users = new List<User>();

            while (table!.Read())
            {
                User user = new User((HostID)Convert.ToInt32(table["HostID"].ToString()), table["ID"].ToString(), table["PW"].ToString());
                users.Add(user);
            }

            table.Close();

            return users;
        }

        public void SelectRoom(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            _Select($"select * from rooms {where}");
        }

        public void SelectRoomUser(string where = "")
        {
            if (where != "")
            {
                if (!where.StartsWith("where") && !where.StartsWith("WHERE"))
                    where = "where " + where;
            }
            _Select($"select * from roomusers {where}");
        }

        private MySqlDataReader? _Select(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, connection);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
