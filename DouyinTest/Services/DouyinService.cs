using DouyinTest.Dao;
using DouyinTest.Entities;
using DouyinTest.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        readonly ILogger logger;

        public DouyinService(IConfiguration config, ILogger<DouyinService> log)
        {
            connectionString = config.GetConnectionString("Mysql");

            domain = config["Douyin:domain"];
            clientKey = config["Douyin:clientKey"];
            clientSecret = config["Douyin:clientSecret"];
            grantType = config["Douyin:grantType"];
            scope = config["Douyin:scope"];
            redirectUri = config["Douyin:redirectUri"];
            pageCount = config.GetValue<int>("Douyin:pageCount");
            logger = log;
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
            try
            {
                TokenDao tokenDao = new TokenDao(connectionString);
                var token = tokenDao.GetAvaliableToken();
                if (token == null)
                {
                    logger.LogWarning("没有可用的Token");
                    return;
                }

                using (var client = HttpClientFactory.Create())
                {
                    string url = $"{domain}/video/list?open_id={token.OpenId}&access_token={token.AccessToken}&cursor={0}&count={pageCount}";
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                    var content = await response.Content.ReadAsStringAsync();

                    var videoListResponse = JsonConvert.DeserializeObject<VideoListResponseModel>(content);
                    if (videoListResponse?.data.error_code != 0)
                    {
                        logger.LogError($"请求Token失败:{videoListResponse?.data?.error_code} {videoListResponse?.data?.description}");
                        return;
                    }

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
            catch (Exception err)
            {
                logger.LogError("获取视频列表失败", err);
            }
        }

        public async Task RefreshToken()
        {
            try
            {
                TokenDao dao = new TokenDao(connectionString);
                var token = dao.GetAvaliableToken();
                if (token == null)
                    return;
                if (DateTime.Now > token.ExpiresIn)
                    return;//已经过期的token无效
                if (DateTime.Now.AddDays(-1) < token.ExpiresIn)
                {// 一天内到期
                    if (token.RefreshTimes >= 5)
                        return;//最多刷新5次
                    await TokenRefreshRequest(token);
                }
            }
            catch (Exception err)
            {
                logger.LogError("刷新Token失败", err);
            }
        }

        public async Task<bool> TokenRefreshRequest(TokenEntity token)
        {
            try
            {
                using (var client = HttpClientFactory.Create())
                {
                    string url = $"{domain}/oauth/renew_refresh_token?client_key={clientKey}&refresh_token={token.RefreshToken}";
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                    var content = await response.Content.ReadAsStringAsync();

                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(content);
                    if (tokenResponse?.data.error_code != 0)
                        return false;

                    var refreshToken = tokenResponse.data.refresh_token;
                    var expiresIn = DateTime.Now.AddSeconds(tokenResponse.data.expires_in);

                    TokenDao dao = new TokenDao(connectionString);
                    return dao.RefreshToken(token.TokenId, refreshToken, expiresIn);
                }
            }
            catch (Exception err)
            {
                logger.LogError("获取Token失败", err);
                return false;
            }
        }

        private DateTime StampToDateTime(long timeStamp)
        {
            var dateStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long lTime = timeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return (dateStart.Add(toNow));
        }
    }
}
