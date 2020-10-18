using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Models
{
    public class VideoModel
    {
        public int VideoId { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 视频创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeString
        {
            get
            {
                if (CreateTime > DateTime.Now.AddHours(-1))
                    return $" {(DateTime.Now - CreateTime).TotalMinutes} 分钟前";
                if (CreateTime > DateTime.Now.AddDays(-1))
                    return $" {(DateTime.Now - CreateTime).TotalHours} 小时前";
                if (CreateTime > DateTime.Now.AddYears(-1))
                    return $" {CreateTime:MM月dd日 HH:mm}";
                return $" {CreateTime:yyyy年MM月dd日}";
            }
        }

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
    }
}
