using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.IO;

namespace DouyinTest.Controllers
{
    public class AuthorityController : Controller
    {
        IConfiguration config;
        readonly ILogger logger;
        DouyinService tokenService;
        public AuthorityController(IConfiguration configuration, ILogger<VideoController> log, DouyinService service)
        {
            config = configuration;
            logger = log;
            tokenService = service;
        }
        public IActionResult Index()
        {
            string url = tokenService.GetAuthUrl();
            return Redirect(url);
        }

        public async Task<IActionResult> Code(string code)
        {
            try
            {
                if (await tokenService.TokenRequest(code))
                {
                    return RedirectToAction("index", "Video");
                }
                return Redirect(config["Douyin:redirectUri"]);
            }
            catch (Exception err)
            {
                logger.LogError(err, "获取Token异常");
                return Redirect(config["Douyin:redirectUri"]);
            }
        }
    }
}
