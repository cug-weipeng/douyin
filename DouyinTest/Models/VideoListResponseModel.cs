using System.Collections.Generic;

namespace DouyinTest.Models
{
    public class VideoListResponseModel
    {
        public DataResponseModel data { get; set; }
        public ExtraResponseModel extra { get; set; }
    }

    public class DataResponseModel
    {
        /// <summary>
        /// 用于下一页请求的cursor
        /// </summary>
        public string cursor { get; set; }

        /// <summary>
        /// 错误码描述	
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public long error_code { get; set; }

        /// <summary>
        /// 是否还有其他视频，配合cursor使用
        /// </summary>
        public bool has_more { get; set; }

        public List<VideoResponseModel> list { get; set; }
    }

    public class VideoResponseModel
    {
        /// <summary>
        /// 视频封面
        /// </summary>
        public string cover { get; set; }

        /// <summary>
        /// 视频创建时间戳
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 表示是否审核结束
        /// </summary>
        public bool is_reviewed { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool is_top { get; set; }

        /// <summary>
        /// 视频id
        /// </summary>
        public string item_id { get; set; }

        /// <summary>
        /// 视频播放页面
        /// </summary>
        public string share_url { get; set; }

        /// <summary>
        /// 统计数据
        /// </summary>
        public StasticsResonseModel statistics { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 表示视频状态。1:已发布;2:不适宜公开;4:审核中
        /// </summary>
        public int video_status { get; set; }
    }

    public class StasticsResonseModel
    {
        /// <summary>
        /// 评论数
        /// </summary>
        public string comment_count { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public string digg_count { get; set; }

        /// <summary>
        /// 下载数
        /// </summary>
        public string download_count { get; set; }

        /// <summary>
        /// 转发数
        /// </summary>
        public string forward_count { get; set; }

        /// <summary>
        /// 播放数
        /// </summary>
        public string play_count { get; set; }

        /// <summary>
        /// 分享数
        /// </summary>
        public string share_count { get; set; }
    }

    public class ExtraResponseModel
    {
        /// <summary>
        /// 标识请求的唯一id
        /// </summary>
        public string logid { get; set; }

        /// <summary>
        /// 毫秒级时间戳
        /// </summary>
        public long now { get; set; }

        /// <summary>
        /// 子错误码描述
        /// </summary>
        public string sub_description { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        public long sub_error_code { get; set; }

        /// <summary>
        /// 错误码描述	
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public long error_code { get; set; }
    }
}
