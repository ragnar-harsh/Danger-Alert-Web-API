using server.Models;

namespace server.Repository
{
    public interface IAuthenticationRepository
    {
        List<dynamic> getAllUsers();
        dynamic AddUser(SignupModel signupModel);
        bool isUserExist(string mob, string DB);
        int otpGenerate(string mob);
        bool otpVerify(string EnteredOTP, string mob);
        string GenerateTokenAsync(TokenModel tokenModel, SigninModel signinModel);
        TokenModel GenerateTokenModelAsync(string mobile);
    }
}