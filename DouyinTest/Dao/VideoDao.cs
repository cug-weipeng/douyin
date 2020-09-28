using Dapper;
using DouyinTest.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace DouyinTest.Dao
{
    public class VideoDao
    {
        const string SQL_SELECT_VIDEO = "SELECT * FROM videos ORDER BY CreateTime desc limit @Offset,@PageSize";
        const string SQL_SELECT_VIDEO_BY_ID = "SELECT * FROM videos WHERE ItemId in @Ids";
        const string SQL_INSERT_VIDEO = "INSERT INTO videos(ItemId,VideoStatus,Title,CreateTime,Cover,ShareUrl,CommentCount,DiggCount,DownloadCount,ForwardCount,PlayCount,ShareCount,ReceiveTime) VALUES(@ItemId,@VideoStatus,@Title,@CreateTime,@Cover,@ShareUrl,@CommentCount,@DiggCount,@DownloadCount,@ForwardCount,@PlayCount,@ShareCount,@ReceiveTime)";

        readonly MySqlConnection connection;
        public VideoDao(string conStr)
        {
            connection = new MySqlConnection(conStr);
        }

        public bool CreateVideo(List<VideoEntity> videos)
        {
            try
            {
                var count = connection.Execute(SQL_INSERT_VIDEO, videos);
                return count != 0;
            }
            catch
            {
                return false;
            }
        }

        public List<VideoEntity> GetVideos(int page, int count)
        {
            return connection.Query<VideoEntity>(SQL_SELECT_VIDEO, new
            {
                Offset = (page - 1) * count,
                PageSize = count
            }).ToList();
        }

        public List<VideoEntity> GetVideosByItemIds(string[] itmesIds)
        {
            return connection.Query<VideoEntity>(SQL_SELECT_VIDEO_BY_ID, new
            {
                Ids = itmesIds
            }).ToList();
        }
    }
}
