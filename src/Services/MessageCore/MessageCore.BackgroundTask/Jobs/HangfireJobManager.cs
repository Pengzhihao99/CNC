using Hangfire;
using MessageCore.BackgroundTask.Jobs.Recurring;

namespace MessageCore.BackgroundTask.Jobs
{
    public static class HangfireJobManager
    {
        public static void ConfigJob()
        {
            //https://www.bejson.com/othertools/cron/

            //RecurringJob.AddOrUpdate<TestJob>("TestJob", x => x.ExecuteAsync(), "0 0/10 * * * ?");

            RecurringJob.AddOrUpdate<SendMessagesJob>("SendMessagesJob", x => x.ExecuteAsync(), "0 0/1 * * * ?");//一分钟执行一次

            RecurringJob.AddOrUpdate<SendMessagesRetryJob>("SendMessagesRetryJob", x => x.ExecuteAsync(), "0 0 */1 * * ?");//一小时执行一次重试
        }
    }
}
