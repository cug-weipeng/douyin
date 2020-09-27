using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Entities
{
    public class VideoEntity
    {
        public int VideoId { get; set; }

        public string ItemId { get; set; }

        /// <summary>
        /// 表示视频状态。1:已发布;2:不适宜公开;4:审核中
        /// </summary>
        public int VideoStatus { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 视频封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 视频播放页面
        /// </summary>
        public string ShareUrl { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 	点赞数
        /// </summary>
        public int DiggCount { get; set; }

        public int DownloadCount { get; set; }

        /// <summary>
        /// 转发数
        /// </summary>
        public int ForwardCount { get; set; }

        /// <summary>
        /// 播放数
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// 分享数
        /// </summary>
        public int ShareCount { get; set; }

        public DateTime ReceiveTime { get; set; }
    }
}
