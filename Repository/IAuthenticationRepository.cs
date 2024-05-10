using server.Models;

namespace server.Repository
{
    public interface IAuthenticationRepository
    {
        List<dynamic> getAllUsers();
        dynamic AddUser(SignupModel signupModel);
        bool isUserExist(string mob, string DB);
        Task<int> otpGenerate(string mob);
        bool otpVerify(string EnteredOTP, string mob);
        string GenerateTokenAsync(TokenModel tokenModel, SigninModel signinModel);
        TokenModel GenerateTokenModelAsync(string mobile);
        void SaveFirebaseId(string Id, string mobile, string user);
        Task SendOTP(int otp, string mob);
    }
}