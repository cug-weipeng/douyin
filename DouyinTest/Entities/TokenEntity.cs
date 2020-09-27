using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DouyinTest.Entities
{
    public class TokenEntity
    {
        public int TokenId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string OpenId { get; set; }

        public int RefreshTimes { get; set; }

        public DateTime ExpiresIn { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
