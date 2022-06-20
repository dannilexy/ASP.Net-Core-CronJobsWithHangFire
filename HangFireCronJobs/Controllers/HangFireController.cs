using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HangFireCronJobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from HangFire Web Api");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult welcome()
        {
           var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to CronJob Apps"));
            return Ok($"Job Id: {jobId}, Email Sent to user");
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            int secondsTime = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Discount email on CronJob Apps"), TimeSpan.FromSeconds(secondsTime));
            return Ok($"Job Id: {jobId}, Discount Email will be sent in {secondsTime} seconds");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult UpdateDatabase()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database Updated"), Cron.Minutely);
            return Ok("Database Check Jobs Initiated");
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult confirm()
        {
            int secondsTime = 30;
            var ParentjobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be Unsubscribed"), TimeSpan.FromSeconds(secondsTime));
            BackgroundJob.ContinueJobWith(ParentjobId, ()=> Console.WriteLine("You were Unsubscribed"));
            
            return Ok($"Confirmation mail sent");
        }


        public void SendWelcomeEmail(string message)
        {
            Console.WriteLine(message);
        }
    }
}
