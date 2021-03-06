﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ZubarskaOrd.Models;

namespace ZubarskaOrd.Repos
{
    class UserRepository
    {

        private static SqlCeConnection connection = DbConnection.Instance.Connection;

        public static bool Login(User user)
        {
            string sql = "SELECT * FROM [LoginFormTable] WHERE [username] ='" + user.Username + "' AND [password] ='" + HashPassword.HashNewPassword(user.Password) + "'";
            SqlCeCommand command = new SqlCeCommand(sql, connection);
            command.Prepare();

            SqlCeDataReader reader = command.ExecuteReader();
            if (user.Username.Length == 0)
                throw new Exception("Username must be entered!");
            else if (user.Password.Length == 0)
                throw new Exception("Password must be entered!");
            else
            {
                if (reader.Read())
                {
                    if (!(reader.IsDBNull(4)))
                    {
                        User.IsAdminUser = true;
                        user.Identity = reader.GetInt32(4);
                    }
                    else
                    { 
                        User.IsAdminUser = false;
                        user.Identity = reader.GetInt32(3);
                    }
                }
                else
                    throw new Exception("Invalid Login please check username and password");
            }
            
            return reader.Read();
        }    
    }
}

