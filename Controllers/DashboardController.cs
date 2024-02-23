using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepo;
        private readonly IAuthenticationRepository _authRepo;

        public DashboardController(IDashboardRepository dashboardRepository, IAuthenticationRepository authenticationRepository)
        {
            _dashboardRepo = dashboardRepository;
            _authRepo = authenticationRepository;
        }


//Remove Member
        [HttpGet("removeMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCode.Status302NotModified)]
        public async Task<IActionResult> RemoveMember([FromQuery]int id, [FromQuery] string mobile)
        {
            if(_dashboardRepo.RemoveMemberAsync(id, mobile)){
                return Ok(new { Message = " Member Removed Successfully "});
            }
            return BadRequest(new { Message = " Some Error Occured"});
        }




//Add Member
        [HttpPost("addMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMember([FromForm] MemberModel memberModel, [FromQuery] string mobile)
        {
            if(_dashboardRepo.AddMemberAsync(memberModel, mobile)){
                return Ok(new { Message = " Member Added Successfully "});
            }
            return BadRequest(new { Message = " Some Error Occured"});
        }



//Get All Member
        [HttpGet("GetAllMember")]
        public async Task<IActionResult> GetAllMember([FromQuery] string mobile)
        {
            var res = _dashboardRepo.GetAllMemberAsync(mobile);
            
            return Ok(res);
        }




////////
/// Alert Controller
///////


// Get All Custom Alerts
        [HttpGet("GetAllAlerts")]
        public async Task<IActionResult> GetAllAlerts([FromQuery]string mobile)
        {
            var res = _dashboardRepo.GetAllAlertsAsync(mobile);
            return Ok(res);
        }


//Remove Alert
        [HttpGet("removeAlert")]
        public async Task<IActionResult> RemoveAlert([FromQuery] int id, [FromQuery] string mobile)
        {
            if(_dashboardRepo.RemoveAlertAsync(id, mobile))
            {
                return Ok(new { Message = " Alert Removed Successfully "});
            }
            return BadRequest(new { Message = " Some Error Occured"});
        }




//Add Custom Alert
        [HttpPost("addAlert")]
        public async Task<IActionResult> AddAlert([FromForm] AlertModel alertModel, [FromQuery] string mobile)
        {
            if(_dashboardRepo.AddAlertAsync(alertModel, mobile))
            {
                return Ok(new { Message = " Alert Added Successfully "});
            }
            return BadRequest(new { Message = " Some Error Occured"});
        }


//Get User Detail
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserDetail([FromQuery] string mobile)
        {
            var res = _dashboardRepo.GetUserDetailAsync(mobile);
            if(res != null)
            {
                return Ok(res);
            }
            return BadRequest(new { Message = " No User is Found "});
        }



        //Generate OTP
        [HttpGet("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp([FromQuery]string mobile)
        {
            var otp = _authRepo.otpGenerate(mobile);
            if(otp != null)
            {
                return Ok(new { Message = "OTP Generated Successfully", Otp = otp});
            }
            return BadRequest();
        }


        // Edit Profile and Verify OTP
        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromForm] string otp, [FromQuery]string mobile, [FromForm] UserModel userModel)
        {
            if(_authRepo.otpVerify(otp, mobile))
            {
                var res = _dashboardRepo.SaveProfileAsync(userModel, mobile);
                if(res)
                {
                    return Ok(new { Message = " Profile Updated Successfully "});
                }
                else
                {
                    Console.WriteLine("Some Error Occured ");
                    return BadRequest();
                }
            }
            else{
                return BadRequest();
            }
        }
    }

}