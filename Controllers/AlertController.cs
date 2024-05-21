using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Helper;
using server.Models;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertController : ControllerBase
    {
        private readonly IAlertRepository _alertRepo;
        public AlertController(IAlertRepository alertRepository)
        {
            _alertRepo = alertRepository;
        }


// Raise Alert and Find the Nearest Service Provider & send Notification
        [HttpPost("AlertRaised")]
        public async Task<IActionResult> RaiseAlert([FromQuery] string mobile, [FromQuery] string type)
        {
            Coordinate ServiceProvider = await _alertRepo.RaiseAlertAsync(mobile, type);
            await _alertRepo.UpdateAlertTableAsync(mobile, ServiceProvider);
            return Ok( new {ServiceProvider, Message = " Alert Raised "});
        }

// Get the Nearest Service Provider
        [HttpPost("GetServiceProvider")]
        public async Task<IActionResult> GetServiceProvider([FromForm]string mobile, [FromForm] string type)
        {
            Coordinate serviceProvider = await _alertRepo.GetServiceProviderAsync(mobile, type);
            return Ok(serviceProvider);
        }


        //Drop Alert 
        [HttpPost("DropAlert")]
        public async Task<IActionResult> DropAlert([FromForm]string mobile, [FromForm] string type)
        {
            await _alertRepo.DropAlertAsync(mobile, type);
            return Ok( new {Message = "Alert Dropped! We hope that you are Safe Now"});
        }


//Raise Custom Alert
        [HttpPost("CustomAlertRaised")]
        public async Task<IActionResult> RaiseCustomAlert([FromForm]string mobile, [FromForm]string title, [FromForm]string message)
        {
            AlertModel CustomAlert = new AlertModel
            {
                id = -1,
                title = title,
                message = message
            };
            await _alertRepo.RaiseCustomAlertAsync(mobile, CustomAlert, 4);
            return Ok( new {Message = " Custom Alert Raised"});
        }


        // [HttpPost("HelpUser")]
        // public async Task<IActionResult> HelpUser()


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
                Console.WriteLine("Some Error");
                return BadRequest();
            }
        }
    }
}