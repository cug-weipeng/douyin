using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DouyinTest.Job
{
    public class QuartzStartup
    {
        public IScheduler _scheduler { get; set; }

        private readonly ILogger _logger;
        private readonly IJobFactory jobfactory;
        private readonly string cron;
        public QuartzStartup(IServiceProvider IocContainer, ILoggerFactory loggerFactory,IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<QuartzStartup>();
            jobfactory = new DouyinJobFactory(IocContainer);
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = jobfactory;
            cron = configuration["Douyin:jobCron"];
        }
        // Quartz.Net启动后注册job和trigger
        public void Start()
        {
            _logger.LogInformation("Schedule job load as application start.");
            _scheduler.Start().Wait();

            var UsageCounterSyncJob = JobBuilder.Create<DouyinJob>()
               .WithIdentity("DouyinJob")
               .Build();

            var UsageCounterSyncJobTrigger = TriggerBuilder.Create()
                .WithIdentity("DouyinCron")
                .StartNow()
                .WithCronSchedule(cron)      // Seconds,Minutes,Hours，Day-of-Month，Month，Day-of-Week，Year（optional field）
                .Build();
            _scheduler.ScheduleJob(UsageCounterSyncJob, UsageCounterSyncJobTrigger).Wait();

            _scheduler.TriggerJob(new JobKey("DouyinJob"));
        }

        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
            else
            {
            }
            _logger.LogCritical("Schedule job upload as application stopped");
        }

    }
}
