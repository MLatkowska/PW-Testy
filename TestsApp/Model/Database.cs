using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace TestsApp.Model
{
    public static class Database
    {
        static Database()
        {
            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=C:\\Users\\Magdalena\\Desktop\\testsDB.sqlite;Version=3;");
            dbConnection.Open();


            dbConnection.Close();
        }



    }
}


