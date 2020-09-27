using DouyinTest.Models;
using System.Collections.Generic;
using DouyinTest.Dao;
using Microsoft.Extensions.Configuration;
using DouyinTest.Utils;

namespace DouyinTest.Services
{
    public class VideoService
    {
        readonly string connectionString;
        public VideoService(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Mysql");
        }

        public List<VideoModel> GetVideoList(int page, int count)
        {
            VideoDao dao = new VideoDao(connectionString);
            var entities = dao.GetVideos(page, count);
            var models = new List<VideoModel>();
            entities?.ForEach(t =>
            {
                models.Add(t.TransformTo<VideoModel>());
            });
            return models;
        }

    }
}
