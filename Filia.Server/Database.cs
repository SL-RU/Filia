using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Zyan.Communication.Security;

namespace Filia.Server
{
    public class Database
    {
        public Database(string path)
        {
            DbConnection = null;
            DbPath = path;
        }

        public SQLiteConnection DbConnection;
        public string DbPath { get; private set; }

        public void ConnectDb()
        {
            DbConnection = new SQLiteConnection(DbPath);
        }

        public void CreateDb()
        {
            DbConnection.CreateTable<DbUserData>();
            DbConnection.CreateTable<DbPhraseData>();
            DbConnection.Commit();
        }

        public void CloseDb()
        {
            DbConnection.Close();
        }

        /// <summary>
        /// Add new user to DB
        /// Use FiliaAuthProvider.CreateUser instead
        /// </summary>
        /// <returns></returns>
        public bool InsertNewUser(DbUserData user)
        {
            if(user == null)
                throw new Exception("Null user object");
            if (string.IsNullOrEmpty(user.Nickname) || user.Password == null)
                throw new Exception("Null nick or password");

            if (DbConnection.Table<DbUserData>().Any(x => x.Nickname == user.Nickname))
            {
                    throw new Exception("Nickname " + user.Nickname + " already exist");
            }

            DbConnection.Insert(user);
            DbConnection.Commit();
            return true;
        }

        public List<DbUserData> GetAllUsers()
        {
            var v = DbConnection.Table<DbUserData>().ToList();
            return v;
        } 
    }
}
