using Dapper;
using DouyinTest.Entities;
using MySql.Data.MySqlClient;
using System;

namespace DouyinTest.Dao
{
    public class TokenDao
    {
        const string SQL_SELECT_TOKEN = "SELECT * FROM tokens WHERE ExpiresIn>@ExpiresIn ORDER BY CreatedTime desc";
        const string SQL_INSERT_TOKEN = "INSERT INTO tokens(AccessToken,RefreshToken,OpenId,RefreshTimes,ExpiresIn,CreatedTime,UpdatedTime) VALUES(@AccessToken,@RefreshToken,@OpenId,@RefreshTimes,@ExpiresIn,@CreatedTime,@UpdatedTime)";
        const string SQL_REFRESH_TOKEN = "UPDATE tokens SET RefreshToken=@RefreshToken,RefreshTimes =RefreshTimes+1, ExpiresIn = @ExpiresIn WHERE TokenId=@TokenId";

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
            return connection.Execute(SQL_INSERT_TOKEN, token) == 1;
        }

        public bool RefreshToken(int tokenid, string newRefreshToken, DateTime expireIn)
        {
            return connection.Execute(SQL_REFRESH_TOKEN, new
            {
                TokenId = tokenid,
                RefreshToken = newRefreshToken,
                ExpiresIn = expireIn
            }) == 1;
        }
    }
}
