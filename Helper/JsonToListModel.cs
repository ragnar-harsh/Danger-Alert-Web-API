using Newtonsoft.Json;
using server.Models;

namespace server.Helper
{
    public class JsonToListModel
    {
        
        public static T DeserializeJson<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }


        public static List<MemberModel> JsonToMember(List<string> list)
        {
            List<MemberModel> MemberList = new List<MemberModel>();
            foreach(string res in list)
            {
                if (!string.IsNullOrEmpty(res))
                {
                    MemberModel deserializedData = DeserializeJson<MemberModel>(res);
                    MemberModel member = new MemberModel(){
                        id = deserializedData.id,
                        name = deserializedData.name,
                        mobile = deserializedData.mobile
                    };
                    MemberList.Add(member);
                }else{
                    MemberList.Add(new MemberModel(){
                        id = -1,
                        name = null,
                        mobile = null
                    });
                }
            }
            return MemberList;
        }


        
        public static List<AlertModel> JsonToAlert(List<string> list)
        {
            List<AlertModel> AlertList = new List<AlertModel>();
            foreach(string res in list)
            {
                if(!string.IsNullOrEmpty(res))
                {
                    AlertModel deserializedData = DeserializeJson<AlertModel>(res);
                    AlertModel alert = new AlertModel(){
                        id = deserializedData.id,
                        title = deserializedData.title,
                        message = deserializedData.message
                    };
                    AlertList.Add(alert);
                }else{
                    AlertList.Add(new AlertModel(){
                        id = -1,
                        title = null,
                        message = null
                    });
                }
            }
            return AlertList;
        }
    }
}