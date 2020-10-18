using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DouyinTest.Models;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DouyinTest.Controllers
{
    public class VideoController : Controller
    {
        readonly VideoService videoService;
        readonly DouyinService douyinService;
        readonly ILogger logger;
        public VideoController(VideoService svc,DouyinService douyinSvc, ILogger<VideoController> log)
        {
            videoService = svc;
            douyinService = douyinSvc;
            logger = log;
        }

        public IActionResult Index()
        {
            try
            {
                var videos = videoService.GetVideoList(1, 10);
                return View(videos);
            }
            catch (Exception err)
            {
                logger.LogError(err, "获取视频异常");
                return View(new List<VideoModel>());
            }
        }

        public async Task<IActionResult> Play(string itemId)
        {
            try
            {
                var url = await douyinService.GetVideoUrl(itemId);
                if (url == null)
                    return Content("<script>alert('获取播放地址失败')</script>");
                return Redirect(url);
            }
            catch (Exception err)
            {
                logger.LogError(err, "获取视频异常");
                return View(new List<VideoModel>());
            }
        }
    }
}
