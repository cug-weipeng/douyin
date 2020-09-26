using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Models
{
    public class TokenResponseModel
    {
        public TokenDataResponseModel data { get; set; }

        public string message { get; set; }
    }

    public class TokenDataResponseModel
    {
        public string scope { get; set; }
        public string unionid { get; set; }
        public string access_token { get; set; }
        public string description { get; set; }
        public long error_code { get; set; }
        public string expires_in { get; set; }
        public string open_id { get; set; }
        public string refresh_token { get; set; }
    }
}
