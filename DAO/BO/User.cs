using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latkowska.Testy.Interfaces;

namespace Latkowska.Tests.DAO.BO
{
    public class User : IUser
    {
        private int _userID;
        private string _login;
        private string _password;
        private bool _editor;

        public User(int userID, string login, string password, bool editor)
        {
            _userID = userID;
            _login = login;
            _password = password;
            _editor = editor;
        }

        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public bool Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                _editor = value;
            }
        }

    }
}
