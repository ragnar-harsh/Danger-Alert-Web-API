using System.Text.Json;
using server.Helper;
using server.Models;
using server.Utilities;

namespace server.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IAuthenticationRepository _authRepo;
        public DashboardRepository(IAuthenticationRepository authenticationRepository)
        {
            _authRepo = authenticationRepository;
        }

        //Get User Detail
        public async Task<UserModel> GetUserDetailAsync(string mobile)
        {
            string query;
            if (_authRepo.isUserExist(mobile, "registered_user_datail"))
            {
                query = string.Format(@"select * from registered_user_datail where mobile = '{0}';", mobile);
            }
            else
            {
                query = string.Format(@"select * from service_provider where mobile = '{0}';", mobile);
            }
            UserModel response = await PGDBUtilityAPI.GetUserFromTable(query);
            return response;

        }


        //Add Member
        public async Task<bool> AddMemberAsync(MemberModel memberModel, string mob)
        {
            int id = memberModel.id;
            string column = "";
            if (id == 1)
            {
                column = "member1";
            }
            else if (id == 2)
            {
                column = "member2";
            }
            else if (id == 3)
            {
                column = "member3";
            }

            string jsonString = JsonSerializer.Serialize(memberModel);

            string query = string.Format(@"UPDATE dashboard_detail SET {0} = '{1}' WHERE mobile = '{2}';", column, jsonString, mob);
            if (await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            return false;
        }

        //Remove Member
        public async Task<bool> RemoveMemberAsync(int id, string mob)
        {
            string column = "";
            if (id == 1)
            {
                column = "member1";
            }
            else if (id == 2)
            {
                column = "member2";
            }
            else if (id == 3)
            {
                column = "member3";
            }
            string query = string.Format(@"UPDATE dashboard_detail SET {0} = NULL WHERE mobile = '{1}';", column, mob);
            if (await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            return false;
        }

        // Get ALL Member
        public async Task<List<MemberModel>> GetAllMemberAsync(string mobile)
        {
            string query = string.Format(@"select member1, member2, member3 from dashboard_detail where mobile = '{0}';", mobile);
            List<string> list = await PGDBUtilityAPI.GetDashboardJsonData(query);
            List<MemberModel> MemberList = JsonToListModel.JsonToMember(list);

            return MemberList;
        }

        //////////
        ///Alert Controller Section
        //////////

        //Add Custom Alerts
        public async Task<bool> AddAlertAsync(AlertModel alertModel, string mob)
        {
            int id = alertModel.id;
            string column = "";
            if (id == 1)
            {
                column = "alert1";
            }
            else if (id == 2)
            {
                column = "alert2";
            }
            else if (id == 3)
            {
                column = "alert3";
            }

            string jsonString = JsonSerializer.Serialize(alertModel);

            string query = string.Format(@"UPDATE dashboard_detail SET {0} = '{1}' WHERE mobile = '{2}';", column, jsonString, mob);
            if (await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            return false;
        }



        //Remove Alert
        public async Task<bool> RemoveAlertAsync(int id, string mob)
        {
            string column = "";
            if (id == 1)
            {
                column = "alert1";
            }
            else if (id == 2)
            {
                column = "alert2";
            }
            else if (id == 3)
            {
                column = "alert3";
            }
            string query = string.Format(@"UPDATE dashboard_detail SET {0} = NULL WHERE mobile = '{1}';", column, mob);
            if (await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            return false;
        }


        // Get ALL Alerts
        public async Task<List<AlertModel>> GetAllAlertsAsync(string mobile)
        {
            string query = string.Format(@"select alert1, alert2, alert3 from dashboard_detail where mobile = '{0}';", mobile);
            List<string> list = await PGDBUtilityAPI.GetDashboardJsonData(query);
            List<AlertModel> AlertList = JsonToListModel.JsonToAlert(list);

            return AlertList;
        }


        //Save Profile
        public async Task<bool> SaveProfileAsync(UserModel userModel, string mobile)
        {

            
            // string filePath = await Upload(userModel.file, userModel.file.FileName);


            string query;
            if (_authRepo.isUserExist(mobile, "registered_user_datail"))
            {
                query = string.Format(@"Update registered_user_datail set name = '{0}', age = '{1}', gender = '{2}', email = '{3}', adhaar = '{4}' where mobile = '{5}'", userModel.name, userModel.age, userModel.gender, userModel.email, userModel.adhaar, mobile);
            }
            else
            {
                query = string.Format(@"Update service_provider set name = '{0}', age = '{1}', gender = '{2}', email = '{3}', adhaar = '{4}' where mobile = '{5}'", userModel.name, userModel.age, userModel.gender, userModel.email, userModel.adhaar, mobile);
            }

            if ( await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Profile Image Updation
        public async Task<string> Upload(IFormFile file, string mobile)
        {
            string fileNew = Guid.NewGuid().ToString() + "_" + file.FileName;
            string location1 = "/assets/profile/";
            string location = "G:/Project 3.0/DangerAlertWebApp/src/assets/profile/";

            var fileNewPath = Path.Combine(location1, fileNew);


            var filePath = Path.Combine(location, fileNew);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string query;
            if (_authRepo.isUserExist(mobile, "registered_user_datail"))
            {
                query = string.Format(@"Update registered_user_datail set profileurl = '{0}' where mobile = '{1}'", fileNewPath, mobile);
            }
            else
            {
                query = string.Format(@"Update service_provider set profileurl = '{0}' where mobile = '{1}'", fileNewPath, mobile);
            }

            await PGDBUtilityAPI.RunUpdateQuery(query);

            return fileNewPath;

        }

    }
}