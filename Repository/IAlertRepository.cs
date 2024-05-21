using server.Helper;
using server.Models;

namespace server.Repository
{
    public interface IAlertRepository
    {
        Task<Coordinate> RaiseAlertAsync(string mobile, string type);
        // Task<string> SendPostRequest(string url, object headers, object bodyParameters);
        Task<bool> UpdateLocationAsync(RoutingModel routingModel);
        Task<RoutingModel> GetLocationAsync(string mobile, string type);
        Task SendNotificationAlert(string firebaseid, int index, string type);
        Task RaiseCustomAlertAsync(string mobile, AlertModel alertModel, int index);
        Task UpdateAlertTableAsync(string mobile, Coordinate coordinate);
        Task<Coordinate> GetServiceProviderAsync(string mobile, string type);
        Task DropAlertAsync(string mobile, string type);
    }
}