using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DouyinTest.Models;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DouyinTest.Controllers
{
    public class VideoController : Controller
    {
        readonly VideoService videoService;
        readonly ILogger logger;
        public VideoController(VideoService svc, ILogger<VideoController> log)
        {
            videoService = svc;
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
    }
}
