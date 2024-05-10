using System.Data;
using System.Net;
using server.Helper;
using server.Models;
using server.Utilities;

namespace server.Repository
{
    public class AlertRepository : IAlertRepository
    {
        private readonly IAuthenticationRepository _authRepo;
        public AlertRepository(IAuthenticationRepository authenticationRepository)
        {
            _authRepo = authenticationRepository;
        }

        private readonly List<NotificationModel> notifications = new List<NotificationModel>
            {
                new NotificationModel
                {
                    Title = "Fire Alert Emergency",
                    Body = "Someone raise Emergency Fire Alert, Needs help ",
                    Icon = "https://t3.ftcdn.net/jpg/02/35/26/30/360_F_235263034_miJw2igmixo7ymCqhHZ7c8wp9kaujzfM.jpg"
                },
                new NotificationModel
                {
                    Title = "Medical Alert Emergency",
                    Body = "Don't forget to provide medical help to your nearby person.",
                    Icon = "https://atlas-content-cdn.pixelsquid.com/stock-images/medical-icon-first-aid-kit-NxPYE2A-600.jpg"
                },
                new NotificationModel
                {
                    Title = "Accident Alert Emergency",
                    Body = "Check out for the road accident in your location",
                    Icon = "https://www.shutterstock.com/image-vector/crashed-cars-vector-600nw-428706862.jpg"
                },
                new NotificationModel
                {
                    Title = "Criminal Alert Emergency",
                    Body = "Check out the location for Criminal Activity",
                    Icon = "https://www.shutterstock.com/image-vector/police-icon-vector-policeman-officer-260nw-1457456297.jpg"
                }
            };

        public async Task<Coordinate> RaiseAlertAsync(string mobile, string type)
        {
            List<Coordinate> providers = new List<Coordinate>();
            double targetLattitude = 38.8951;
            double targetLongitude = -77.0364;
            string query = string.Format(@"select name, mobile, firebaseid, lattitude, longitude from service_provider where department = '{0}';", type);
            DataTable dt = PGDBUtilityAPI.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Coordinate cord = new Coordinate(dr["name"].ToString(), dr["mobile"].ToString(), dr["firebaseid"].ToString(), Convert.ToDouble(dr["lattitude"]), Convert.ToDouble(dr["longitude"]));
                    providers.Add(cord);
                }
                Coordinate currServProvider = await NearestCoordinateFinder.FindNearestCoordinate(targetLattitude, targetLongitude, providers);
                // Console.WriteLine(currServProvider.Name + " " + currServProvider.Firebaseid + " ");

                int index = -1;
                if(type == "Medical"){
                    index = 1;
                }
                else if(type == "Fire"){
                    index = 0;
                }
                else if(type == "Police"){
                    index = 3;
                }
                else if(type == "Traffic"){
                    index = 2;
                }

                await SendNotificationAlert(currServProvider.Firebaseid, index, type);
                return currServProvider;

            }
            return null;


        }


// Send Notification to Users
        public async Task SendNotificationAlert(string firebaseid, int index, string type)
        {
            try
            {

                string apiUrl = "https://fcm.googleapis.com/fcm/send";

                var headers = new
                {
                    ContentType = "application/json",
                    Authorization = "Bearer AAAANf6fV9g:APA91bHTM8wokUAqvKZXygcFc0hvyV6UJkf1LAiUpT2xWmdanUsovO8LWp_CyuVMIqFY2H3i_jFUZk1PsZL0ugR7yvwuHSSgX93IigqFPqtqcoNPK_K1c7SDVUluMuKWdWj_yT4JffFq"
                };


                var bodyParameters = new
                {
                    to = firebaseid,
                    notification = new
                    {
                        title = notifications[index].Title,
                        body = notifications[index].Body,
                        icon = notifications[index].Icon
                    }
                };
                // var bodyParameters = new
                // {
                //     to = "d8deC6sxwwDz7ck8nfzaf2:APA91bFheo1RsT4Yp2SVvkn5ZPlZJk-66_XcrfLK-V0a8JNELdjx0iTtgAr5ypB2Iv41e50VAIBNH2AwSTaUCgGj1rYrndsMoK7b2JTJGhlLoEtGsGq8h1sp6XKyl7xjb2cu5bu6tKGh",
                //     notification = new
                //     {
                //         title = "Testing Notification",
                //         body = "Body of Notification",
                //         icon = "https://i.ibb.co/Mkx9z2k/peakpx-3.jpg",
                //         image = "https://i.ibb.co/Mkx9z2k/peakpx-3.jpg"
                //     }
                // };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyParameters);

                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                wc.Headers.Add("Authorization", "Bearer AAAANf6fV9g:APA91bHTM8wokUAqvKZXygcFc0hvyV6UJkf1LAiUpT2xWmdanUsovO8LWp_CyuVMIqFY2H3i_jFUZk1PsZL0ugR7yvwuHSSgX93IigqFPqtqcoNPK_K1c7SDVUluMuKWdWj_yT4JffFq");
                wc.Encoding = System.Text.Encoding.UTF8;
                string res = wc.UploadString(apiUrl, json);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<bool> UpdateLocationAsync(RoutingModel routingModel)
        {
            string tableName = _authRepo.isUserExist(routingModel.Mobile, "registered_user_datail") ? "registered_user_datail" : "service_provider";

            string query = string.Format(@"UPDATE {0} SET lattitude = '{1}', longitude = '{2}' WHERE mobile = '{3}';", tableName, routingModel.Lattitude, routingModel.Longitude, routingModel.Mobile);

            if (await PGDBUtilityAPI.RunUpdateQuery(query))
            {
                return true;
            }
            else
            {
                return false;
            }
        }




     // Alternate function to send Notification
        // public async Task<string> SendPostRequest(string url, object headers, object bodyParameters)
        // {
        //     using (HttpClient client = new HttpClient())
        //     {
        //         // Set headers
        //         foreach (var property in headers.GetType().GetProperties())
        //         {
        //             client.DefaultRequestHeaders.Add(property.Name, property.GetValue(headers).ToString());
        //         }

        //         // Convert body parameters to JSON
        //         string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(bodyParameters);
        //         // Console.WriteLine(jsonBody);

        //         jsonBody = "{\"to\":\"fxI0y1KZslztKprv-1VrUy:APA91bFCgJM34PIvn6MFbtB8bXUxqmojolOEh1sBoI53oMXg32TQU-au8ra5_Z9vYpM_EFM5KoDxKn80NzXouEMPUnamZ5_Li1j1VDtZlPiA6a15UhxU-L1IaoAMtCUODoD9YpDT0Ppj\",\"notification\" : {\"title\" : \"Testing Notification\",\"body\" : \"Body of Notification\", \"icon\" : \"https://i.ibb.co/Mkx9z2k/peakpx-3.jpg\", \"image\" : \"https://i.ibb.co/Mkx9z2k/peakpx-3.jpg\"  } }  ";
        //         // Create HttpContent from JSON
        //         HttpContent content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //         // Send POST request
        //         HttpResponseMessage response = await client.PostAsync(url, content);

        //         // Check if the response is successful
        //         response.EnsureSuccessStatusCode();

        //         // Read and return the response content
        //         return await response.Content.ReadAsStringAsync();
        //     }
        // }
    }
}