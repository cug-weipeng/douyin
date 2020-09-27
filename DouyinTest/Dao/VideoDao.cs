using Dapper;
using DouyinTest.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Dao
{
    public class VideoDao
    {
        const string SQL_SELECT_VIDEO = "SELECT * FROM videos WHERE ORDER BY CreateTime desc limit @Offset,@PageSize";
        const string SQL_INSERT_VIDEO = "INSERT INTO Video(ItemId,VideoStatus,Title,CreateTime,Cover,ShareUrl,CommentCount,DiggCount,DownloadCount,ForwardCount,PlayCount,ShareCount,ReceiveTime) VALUES(@ItemId,@VideoStatus,@Title,@CreateTime,@Cover,@ShareUrl,@CommentCount,@DiggCount,@DownloadCount,@ForwardCount,@PlayCount,@ShareCount,@ReceiveTime)";

        readonly MySqlConnection connection;
        public VideoDao(string conStr)
        {
            connection = new MySqlConnection(conStr);
        }

        public bool CreateVideo(VideoEntity video)
        {
            return connection.Execute(SQL_INSERT_VIDEO, video) == 1;
        }
        public List<VideoEntity> GetVideos(int page,int count)
        {
            return connection.Query<VideoEntity>(SQL_INSERT_VIDEO, new
            {
                Offset = (page - 1)* count,
                PageSize = count
            }).ToList();
        }
    }
}
