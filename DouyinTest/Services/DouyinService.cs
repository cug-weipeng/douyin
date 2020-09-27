using DouyinTest.Dao;
using DouyinTest.Entities;
using DouyinTest.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DouyinTest.Services
{
    public class DouyinService
    {
        readonly string connectionString;
        readonly string clientKey;
        readonly string clientSecret;
        readonly string grantType;
        readonly string domain;
        readonly string scope;
        readonly string redirectUri;
        readonly int pageCount;

        public DouyinService(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Mysql");

            domain = config["Douyin:domain"];
            clientKey = config["Douyin:clientKey"];
            grantType = config["Douyin:grantType"];
            scope = config["Douyin:scope"];
            redirectUri = config["Douyin:redirectUri"];
            pageCount = config.GetValue<int>("Douyin:pageCount");
        }

        public async Task<bool> TokenRequest(string code)
        {
            using (var client = HttpClientFactory.Create())
            {
                string url = $"{domain}/oauth/access_token?client_key={clientKey}&client_secret={clientSecret}&code={code}&grant_type={grantType}";
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var content = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(content);
                if (tokenResponse?.data.error_code != 0)
                    return false;

                var token = new TokenEntity()
                {
                    AccessToken = tokenResponse.data.access_token,
                    OpenId = tokenResponse.data.open_id,
                    RefreshToken = tokenResponse.data.refresh_token,
                    RefreshTimes = 0,
                    ExpiresIn = DateTime.Now.AddSeconds(tokenResponse.data.expires_in),
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                };

                TokenDao dao = new TokenDao(connectionString);
                return dao.CreateToken(token);
            }

        }

        public string GetAuthUrl()
        {
            string state = Guid.NewGuid().ToString();
            var url = ($"{domain}/platform/oauth/connect?client_key={clientKey}&response_type={grantType}&scope={scope}&redirect_uri={redirectUri}&state={state}");
            return url;
        }

        public async Task DownloadVideoList()
        {
            TokenDao tokenDao = new TokenDao(connectionString);
            var token = tokenDao.GetAvaliableToken();
            if (token == null)
                return;

            using (var client = HttpClientFactory.Create())
            {
                string url = $"{domain}/video/list?open_id={token.OpenId}&access_token={token.AccessToken}&cursor={0}&count={pageCount}";
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var content = await response.Content.ReadAsStringAsync();

                var videoListResponse = JsonConvert.DeserializeObject<VideoListResponseModel>(content);
                if (videoListResponse?.data.error_code != 0)
                    return;

                List<VideoEntity> entities = new List<VideoEntity>();
                var dao = new VideoDao(connectionString);
                videoListResponse?.data?.list?.ForEach(t =>
                {
                    entities.Add(new VideoEntity()
                    {
                        ItemId = t.item_id,
                        VideoStatus = t.video_status,
                        Title = t.title,
                        CreateTime = StampToDateTime(t.create_time),
                        Cover = t.cover,
                        ShareUrl = t.share_url,
                        CommentCount = t.statistics.comment_count,
                        DiggCount = t.statistics.digg_count,
                        DownloadCount = t.statistics.download_count,
                        ForwardCount = t.statistics.forward_count,
                        PlayCount = t.statistics.play_count,
                        ShareCount = t.statistics.share_count,
                        ReceiveTime = DateTime.Now
                    });
                });

                if (entities.Count == 0)
                    return;

                var existIds = dao.GetVideosByItemIds(entities.Select(t => t.ItemId).ToArray())
                    .Select(t => t.ItemId)
                    .ToArray();

                var newEntities = entities
                   .Where(t => !existIds.Contains(t.ItemId))
                   .ToList();

                dao.CreateVideo(newEntities);
            }
        }

        private DateTime StampToDateTime(long timeStamp)
        {
            long lTime = timeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return (new DateTime(1970, 1, 1)).Add(toNow);
        }
    }
}
