using DouyinTest.Services;
using Quartz;
using System.Threading.Tasks;

namespace DouyinTest.Job
{
    public class DouyinJob : IJob
    {
        readonly DouyinService tokenService;
        public DouyinJob(DouyinService tokenSvc)
        {
            tokenService = tokenSvc;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await tokenService.RefreshToken();
            await tokenService.DownloadVideoList();
        }
    }
}
