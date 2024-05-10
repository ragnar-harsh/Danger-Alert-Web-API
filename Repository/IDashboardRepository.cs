using server.Models;

namespace server.Repository
{
    public interface IDashboardRepository
    {
        Task<bool> AddMemberAsync(MemberModel memberModel, string mobile);

        Task<bool> RemoveMemberAsync(int id, string mob);

        Task<List<MemberModel>> GetAllMemberAsync(string mobile);

        Task<bool> AddAlertAsync(AlertModel alertModel, string mobile);

        Task<bool> RemoveAlertAsync(int id, string mob);

        Task<List<AlertModel>> GetAllAlertsAsync(string mobile);

        Task<UserModel> GetUserDetailAsync(string mobile);

        Task<bool> SaveProfileAsync(UserModel userModel, string mobile);
        Task<string> Upload(IFormFile file, string mobile);
    }
}