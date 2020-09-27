using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DouyinTest.Models;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DouyinTest.Controllers
{
    public class VideoController : Controller
    {
        readonly VideoService videoService;
        public VideoController(VideoService svc) => videoService = svc;

        public async Task<IActionResult> Index()
        {
            var videos = videoService.GetVideoList(0,10);
            return View(videos);
        }
    }
}
