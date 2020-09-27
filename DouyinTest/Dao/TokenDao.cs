using Dapper;
using DouyinTest.Entities;
using DouyinTest.Services;
using MySql.Data.MySqlClient;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DouyinTest.Dao
{
    public class TokenDao
    {
        const string SQL_SELECT_TOKEN = "SELECT * FROM tokens WHERE ExpiresIn>@ExpiresIn ORDER BY CreatedTime desc";
        const string SQL_INSERT_TOKEN = "INSERT INTO tokens(AccessToken,RefreshToken,OpenId,RefreshTimes,ExpiresIn,CreatedTime,UpdatedTime) VALUES(@AccessToken,@RefreshToken,@OpenId,@RefreshTimes,@ExpiresIn,@CreatedTime,@UpdatedTime)";

        readonly MySqlConnection connection;
        public TokenDao(string conStr)
        {
            connection = new MySqlConnection(conStr);
        }
        public TokenEntity GetAvaliableToken()
        {
            return connection.QueryFirstOrDefault<TokenEntity>(SQL_SELECT_TOKEN, new
            {
                ExpiresIn = DateTime.Now
            });
        }

        public bool CreateToken(TokenEntity token)
        {
            return connection.Execute(SQL_INSERT_TOKEN, token)==1;
        }
    }
}
