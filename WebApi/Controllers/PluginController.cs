using AppService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/plugin")]
    public class PluginController : ControllerBase, IReporter
    {
        public PluginController()
        {

        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromForm] Request request)
        {
            request.Path = "C:\\Users\\Mohammad\\Desktop\\DB\\Big Test.txt";
            var plugin = new Plugin();
            Guid projectHandle = new Guid("bc6e0014-2281-47f7-8925-1cf3056d20b5");
            await Task.Factory.StartNew(() => { plugin.Start(projectHandle, request.Path, this); });
            return Ok();
        }

        [HttpPost("stop")]
        public async Task<IActionResult> Stop([FromForm] Request request)
        {
            request.Path = "C:\\Users\\Mohammad\\Desktop\\DB\\Big Test.txt";
            var plugin = new Plugin();
            Guid projectHandle = new Guid("bc6e0014-2281-47f7-8925-1cf3056d20b5");
            await Task.Factory.StartNew(() => { plugin.Stop(projectHandle); });
            return Ok();
        }

        [NonAction]
        public void LogStatus(string result)
        {
            // We can use signalR here to report results to users
        }

        [NonAction]
        public void LogProgress(decimal percentage)
        {
            // We can use signalR here to report results to users
        }
    }
}