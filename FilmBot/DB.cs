using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FilmBot
{
    class DB
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root; database=filmpoisk");

        private string _sql { get; set; } = "SELECT * FROM `filmdescription`"; 

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public string GetFromBD(string sql)
        {
            string res = "";

            MySqlCommand cm = new MySqlCommand(sql,GetConnection());
            
            MySqlDataReader reader = cm.ExecuteReader();

            while (reader.Read())
            {
                res = reader[0].ToString() + "\n" + reader[1].ToString();
            }

            reader.Close(); // закрываем read 
            
            return res;
        }
        
        public string SelectFromBD(string sql)
        {
            string res;
            _sql = sql;
            //string sql = $"SELECT `name`,`descript` FROM `filmdescription` WHERE name={film}";
            this.openConnection();
            res = GetFromBD(sql);
            this.closeConnection();
            return res;
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
