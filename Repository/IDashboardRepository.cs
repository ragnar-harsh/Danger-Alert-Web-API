using server.Models;

namespace server.Repository
{
    public interface IDashboardRepository
    {
        bool AddMemberAsync(MemberModel memberModel, string mobile);

        bool RemoveMemberAsync(int id, string mob);

        List<MemberModel> GetAllMemberAsync(string mobile);

        
        bool AddAlertAsync(AlertModel alertModel, string mobile);

        bool RemoveAlertAsync(int id, string mob);

        List<AlertModel> GetAllAlertsAsync(string mobile);

        UserModel GetUserDetailAsync(string mobile);

        bool SaveProfileAsync(UserModel userModel, string mobile);
    }
}