using DouyinTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DouyinTest.Dao;
using Microsoft.Extensions.Configuration;

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
            
        }

    }
}
