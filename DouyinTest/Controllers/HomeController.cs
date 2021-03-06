﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DouyinTest.Models;
using System.Web;
using DouyinTest.Services;
using Microsoft.Extensions.Configuration;

namespace DouyinTest.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return RedirectToAction("index", "Video");

            //string clientKey = "awxhj33sclgkxh22";
            //string responseType = "code";
            //string scope = "video.list,video.data";
            //string redirectUri = "http://www.zjtoprs.com/";
            //string state = "state_example";

            //var url =($"{domain}/platform/oauth/connect?client_key={clientKey}&response_type={responseType}&scope={scope}&redirect_uri={redirectUri}&state={state}");
            //return Redirect(url);
        }

        public IActionResult Code(string code)
        {
            return Redirect("http://www.zjtoprs.com/");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
