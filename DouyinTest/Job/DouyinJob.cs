using DouyinTest.Services;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Cms;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Job
{
    public class DouyinJob : IJob
    {
        readonly IConfiguration config;
        readonly DouyinService tokenService;
        public DouyinJob(IConfiguration configuration, DouyinService tokenSvc)
        {
            config = configuration;
            tokenService = tokenSvc;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return tokenService.DownloadVideoList();
        }
    }
}
