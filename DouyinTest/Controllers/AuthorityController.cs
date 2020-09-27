using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DouyinTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Utilities.IO;

namespace DouyinTest.Controllers
{
    public class AuthorityController : Controller
    {
        IConfiguration config;
        public AuthorityController(IConfiguration configuration) => config = configuration;
        public IActionResult Index()
        {
            DouyinService tokenService = new DouyinService(config);
            string url = tokenService.GetAuthUrl();
            return Redirect(url);
        }

        public async Task<IActionResult> Code(string code)
        {
            DouyinService service = new DouyinService(config);
            if (await service.TokenRequest(code))
            {
                return RedirectToAction("index", "Video");
            }
            return Redirect("http://www.zjtoprs.com/");
        }

    }
}
