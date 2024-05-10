using Microsoft.AspNetCore.Mvc;
using server.Helper;
using server.Models;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertRepository _alertRepo;
        public AlertController(IAlertRepository alertRepository)
        {
            _alertRepo = alertRepository;
        }


        [HttpPost("AlertRaised")]
        public async Task<IActionResult> RaiseAlert([FromQuery] string mobile, [FromQuery] string type)
        {
            Console.WriteLine("function called " + type);
            Coordinate ServiceProvider = await _alertRepo.RaiseAlertAsync(mobile, type);
            return Ok( new {ServiceProvider, Message = " Alert Raised "});
        }

        [HttpPost("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation([FromForm] RoutingModel routingModel)
        {
            var res = await _alertRepo.UpdateLocationAsync(routingModel);
            if(res)
            {
                return Ok(new { Message = "Updated Successfully"});
            }
            else
            {
                Console.WriteLine("SomeError");
                return BadRequest();
            }
        }
    }
}