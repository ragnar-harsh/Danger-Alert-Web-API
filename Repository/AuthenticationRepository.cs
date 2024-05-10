using System.Data;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.Utilities;
using Vonage;
using Vonage.Request;

namespace server.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        // private readonly IConfiguration _configuration;
        // public AuthenticationRepository(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }

        //Get All User List
        public List<dynamic> getAllUsers()
        {
            List<dynamic> lstUser = new List<dynamic>();
            string qry = string.Format(@"select name, mobile from registered_user_datail");
            DataTable dt1 = PGDBUtilityAPI.GetDataTable(qry);
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    dynamic objct = new ExpandoObject();
                    objct.name = dr["Name"].ToString();
                    objct.mobile = dr["Mobile"].ToString();
                    lstUser.Add(objct);
                }
            }
            return lstUser;
        }


        //Add User 
        public dynamic AddUser(SignupModel signupModel)
        {
            dynamic tUsers = new ExpandoObject();
            if (string.IsNullOrEmpty(signupModel.department) || signupModel.department == "null")
            {
                string strquery = string.Format(@"insert into registered_user_datail(name, age, email, mobile, gender) 
                values ('{0}','{1}','{2}','{3}','{4}')", signupModel.name, signupModel.age, signupModel.email, signupModel.mobile, signupModel.gender);
                int r = PGDBUtilityAPI.ExecuteCommand(strquery);
                if (r > 0)
                {
                    tUsers.NameXCV = signupModel.name;
                    tUsers.mobileUIO = signupModel.mobile;
                }
            }
            else
            {
                string strquery = string.Format(@"insert into service_provider(name, age, email, mobile, gender, department) 
                values ('{0}','{1}','{2}','{3}','{4}', '{5}')", signupModel.name, signupModel.age, signupModel.email, signupModel.mobile, signupModel.gender, signupModel.department);
                int r = PGDBUtilityAPI.ExecuteCommand(strquery);
                if (r > 0)
                {
                    tUsers.NameXCV = signupModel.name;
                    tUsers.mobileUIO = signupModel.mobile;
                }
            }
            string query = string.Format(@"insert into dashboard_detail(mobile) values ('{0}')", signupModel.mobile);
            PGDBUtilityAPI.ExecuteCommand(query);
            return tUsers;

        }


        //Check User Exist or Not
        public bool isUserExist(string mob, string DB)
        {
            string qry = string.Format(@"select * from {1} where mobile = '{0}'", mob, DB);
            DataTable dt = PGDBUtilityAPI.GetDataTable(qry);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }


        //Generate OTP
        public async Task<int> otpGenerate(string mob)
        {
            Random rd = new Random();
            int otp = rd.Next(1000, 9999);
            if (isUserExist(mob, "otp_record"))
            {
                string qry = string.Format(@"delete from otp_record where mobile = '{0}'", mob);
                PGDBUtilityAPI.ExecuteCommand(qry);
            }
            string strqry = string.Format(@"insert into otp_record(mobile, current_otp) values ('{0}','{1}')", mob, otp);
            PGDBUtilityAPI.ExecuteCommand(strqry);
            // Console.WriteLine(otp);

            //Vonage OTP ApI

            // try{
            //     await SendOTP(otp, mob);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.ToString(), ex.Message);
            // }

            return otp;
        }


        //Verify OTP
        public bool otpVerify(string EnteredOTP, string mob)
        {
            string qry = string.Format(@"DELETE FROM otp_record WHERE timestamp < CURRENT_TIMESTAMP - INTERVAL '2 minutes';");
            PGDBUtilityAPI.ExecuteCommand(qry);
            string strqry = string.Format(@"select current_otp from otp_record where mobile = '{0}'", mob);

            string otp = PGDBUtilityAPI.GetStringFromTable(strqry);
            if (otp == EnteredOTP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Generate Token Model
        public TokenModel GenerateTokenModelAsync(string mobile)
        {
            if (isUserExist(mobile, "registered_user_datail"))
            {
                string qry = string.Format(@"select name from registered_user_datail where mobile = '{0}'", mobile);
                string Name = PGDBUtilityAPI.GetStringFromTable(qry);
                var tokenModel = new TokenModel()
                {
                    name = Name,
                    role = "Service User"
                };
                return tokenModel;
            }
            else
            {
                string qry = string.Format(@"select name, department from service_provider where mobile = '{0}'", mobile);
                TokenModel tokenModel = PGDBUtilityAPI.getRowFromTable(qry);
                return tokenModel;
            }
        }


        // Generate JWT Token 
        public string GenerateTokenAsync(TokenModel tokenModel, SigninModel signinModel)
        {
            // var authClaims = new List<Claim>{
            //     new Claim(ClaimTypes.Name, signinModel.mobile),
            //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // };
            // var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            // var token = new JwtSecurityToken(
            //     issuer: _configuration["JWT:ValidIssuer"],
            //     audience: _configuration["JWT:ValidAudience"],
            //     expires: DateTime.Now.AddDays(1),
            //     claims: authClaims,
            //     signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            // );
            // return new JwtSecurityTokenHandler().WriteToken(token);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("DangerAlertAppSecureKey012ForSecurity1234567890afafkhakfhahf");
            var identity = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Name, tokenModel.name),
                new Claim(ClaimTypes.Role, tokenModel.role),
                new Claim(ClaimTypes.SerialNumber, signinModel.mobile)
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }



        // Save FireBase Id 
        public void SaveFirebaseId(string Id, string mobile, string user)
        {
            if(Id == "undefined")
            {
                Id = "d8deC6sxwwDz7ck8nfzaf2:APA91bFheo1RsT4Yp2SVvkn5ZPlZJk-66_XcrfLK-V0a8JNELdjx0iTtgAr5ypB2Iv41e50VAIBNH2AwSTaUCgGj1rYrndsMoK7b2JTJGhlLoEtGsGq8h1sp6XKyl7xjb2cu5bu6tKGh";
            }
            string query;
            if (user == "Service User")
            {
                query = string.Format(@"update registered_user_datail set firebaseid = '{0}' where mobile = '{1}';", Id, mobile);
            }
            else
            {
                query = string.Format(@"update service_provider set firebaseid = '{0}' where mobile = '{1}';", Id, mobile);
            }
            PGDBUtilityAPI.ExecuteCommand(query);

            return;
        }



        //Vonage OTP API

        public async Task SendOTP(int otp, string mob)
        {
            var credentials = Credentials.FromApiKeyAndSecret(
            "c67c5a62",
            "58ibzmabAys1UZqp"
            );

            var VonageClient = new VonageClient(credentials);


            var response = await VonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
            {
                To = "91" + mob,
                From = "Vonage APIs",
                Text = "Hi there, Your OTP is " + otp.ToString()
            });
            // Console.WriteLine(response.Messaging.SendSmsResponse);

        }




















        // public void sendOTP(int otp, string mobile)
        // {

        //     string accountSid = "AC175d35020b382579535927dde4ca105a";
        //     string authToken = "4e6ec96306ef309faa72100cb690b54a";
        //     // string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        //     // string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

        //     TwilioClient.Init(accountSid, authToken);

        //     var message = MessageResource.Create(
        //         body: "Hi there, this is Your OTP " ,
        //         // from: new Twilio.Types.PhoneNumber("+15017122661"),
        //         to: new Twilio.Types.PhoneNumber("+919506703301")
        //     );

        //     Console.WriteLine(message.Sid);
        // }
    }
}