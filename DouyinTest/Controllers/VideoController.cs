using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DouyinTest.Models;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace DouyinTest.Controllers
{
    public class VideoController : Controller
    {

        public async Task<IActionResult> Index()
        {

            VideoService service = new VideoService();

            var result = await service.GetVideoList();

            return View(result);
            return View(new List<VideoModel>() {
                new VideoModel(){
                    Id = "1",
                    Title = "视频1",
                    Cover = "",
                    Url = "   https://vfx.mtime.cn/Video/2019/01/15/mp4/190115161611510728_480.mp4"
                },
                 new VideoModel(){
                    Id = "2",
                    Title = "视频2",
                    Cover = "",
                    Url = "https://media.w3.org/2010/05/sintel/trailer.mp4"
                }
            });
        }
    }
}
