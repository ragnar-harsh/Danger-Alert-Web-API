using server.Helper;
using server.Models;

namespace server.Repository
{
    public interface IAlertRepository
    {
        Task<Coordinate> RaiseAlertAsync(string mobile, string type);
        // Task<string> SendPostRequest(string url, object headers, object bodyParameters);
        Task<bool> UpdateLocationAsync(RoutingModel routingModel);
        Task SendNotificationAlert(string firebaseid, int index, string type);
    }
}