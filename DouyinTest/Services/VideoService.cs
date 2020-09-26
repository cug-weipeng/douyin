using DouyinTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DouyinTest.Services
{
    public class VideoService
    {
        readonly string domain = $"https://open.douyin.com";

        readonly int count = 10;
        readonly string clientKey = "awxhj33sclgkxh22";
        readonly string clientSecret = "8970a48866fe92c13f8ba40a0751d54c";
        readonly string grantType = "authorization_code";

        //public static string token = null;
        public static string token = "act.5b9904da0b0604c02be334c467e123001Bb0SNz23f9JDlKWnFW3BoidghAw";
        private static string openId = "417d2555-85d0-41d5-b0a3-130b20001a8f";

        public async Task<List<VideoModel>> GetVideoList()
        {
            using (var client = HttpClientFactory.Create())
            {
                string url = $"{domain}/video/list?open_id={openId}&access_token={token}&cursor={0}&count={count}";
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var content = await response.Content.ReadAsStringAsync();

                var videoListResponse = JsonConvert.DeserializeObject<VideoListResponseModel>(content);
                if (videoListResponse?.data.error_code != 0)
                    return new List<VideoModel>();
                var videos = new List<VideoModel>();
                videoListResponse?.data?.list?.ForEach(t =>
                {
                    videos.Add(new VideoModel()
                    {
                        Cover = t.cover,
                        Id = t.item_id,
                        Title = t.title,
                        Url = t.share_url
                    });
                });
                return videos;

            }
        }


        public async Task<bool> GetToken(string code)
        {
            using (var client = HttpClientFactory.Create())
            {
                string url = $"{domain}/oauth/access_token?client_key={clientKey}&client_secret={clientSecret}&code={code}&grant_type={grantType}";
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var content = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(content);
                if (tokenResponse?.data.error_code != 0)
                    return false;

                token = tokenResponse.data.access_token;
                openId = tokenResponse.data.open_id;

                return true;

            }

        }
    }
}
