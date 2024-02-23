using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthenticationController(IAuthenticationRepository authenticationRepository) : ControllerBase {

        private readonly IAuthenticationRepository _IAuthenticationRepo = authenticationRepository;


//Get User List
        [HttpGet("userList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult getUserr(){
            var users = _IAuthenticationRepo.getAllUsers();

            return Ok(users);
        }


//Generate Login OTP
        [HttpGet("genLoginOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> OtpGenLogin([FromQuery] string mob){
            if(_IAuthenticationRepo.isUserExist(mob, "registered_user_datail") || _IAuthenticationRepo.isUserExist(mob, "service_provider")){
                int generatedOtp = _IAuthenticationRepo.otpGenerate(mob);
                return Ok(new { Message = "OTP Generated Successfully", otp = generatedOtp} );
            }        
            return Unauthorized(new { Message = "User Not Registered. Please Sign Up first."} );
        }


//Login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> loginValidation([FromForm] SigninModel signinModel){
            if(!_IAuthenticationRepo.otpVerify(signinModel.otp, signinModel.mobile)){
                return  Unauthorized(new { Message = " Incorrect OTP "});
            }
            else{
                TokenModel tokenModel = _IAuthenticationRepo.GenerateTokenModelAsync(signinModel.mobile);
                var token = _IAuthenticationRepo.GenerateTokenAsync(tokenModel, signinModel);
                // Console.WriteLine(token.Result.ToString());
                // return Ok(token);
                return Ok(new { Token = token, Message = "Login Successfully "});
            }
        }   


// Generate Signup OTP
        [HttpGet("genSignupOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> OtpGen([FromQuery] string mob){

            if(mob != null){
                int otp = _IAuthenticationRepo.otpGenerate(mob);
                return Ok(new { Message = "OTP Generated Successfully", Otp = otp} );
            }
            else{
                return BadRequest( new { Message = " User already Registered "} );
            }
        }


//Register User
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> registerUser([FromForm] SignupModel signupModel){
            if(!_IAuthenticationRepo.otpVerify(signupModel.otp, signupModel.mobile)){
                return Unauthorized(new { Message = "Incorrect OTP" });
            }
            var res = _IAuthenticationRepo.AddUser(signupModel);
            return Ok(new { Message = " User Registered Successfully "});

        }

    }
}