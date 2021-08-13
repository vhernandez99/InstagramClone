using InstagramClone.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace InstagramClone.Services
{
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string userName, string password, MediaFile file, byte[] userImageArray)
        {
            string tokenFirebase = Preferences.Get("TokenFirebase", string.Empty);

            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent
            {
                {new StringContent(name),"Name" },
                {new StringContent(userName),"UserName" },
                {new StringContent(password),"Password" },
                {new StringContent(tokenFirebase),"TokenFirebase" },
            };
            content.Add(new StreamContent(new MemoryStream(userImageArray)), "Image", file.Path);
            var ApiResponse = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/Register", content);
            if (!ApiResponse.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> AddPost(string description, MediaFile file, byte[] postImageArray)
        {
            await TokenValidator.CheckTokenValidity();
            var id = Preferences.Get("userId", 0);
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent
            {
                { new StringContent(description), "Description" }
            };
            content.Add(new StreamContent(new MemoryStream(postImageArray)), "Image", file.Path);
            var ApiResponse = await httpClient.PostAsync(AppSettings.ApiUrl + "api/posts/AddPost/" + id, content);
            if (!ApiResponse.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<bool> Login(string user, string password)
        {
            string tokenFirebase = Preferences.Get("TokenFirebase", string.Empty);
            var login = new User()
            {
                UserName = user,
                Password = password,
                TokenFirebase = tokenFirebase
            };
            var httpclient = new HttpClient();
            var jsonRegister = JsonConvert.SerializeObject(login);
            var content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var ApiResponse = await httpclient.PostAsync(AppSettings.ApiUrl + "api/users/login", content);
            if (!ApiResponse.IsSuccessStatusCode) return false;
            var jsonresult = await ApiResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonresult);
            Preferences.Set("accessToken", result.Access_token);
            Preferences.Set("userId", result.User_Id);
            Preferences.Set("userName", result.User_Name);
            Preferences.Set("tokenExpirationTime", result.Expiration_Time);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            return true;
        }
        public static async Task<List<Post>> GetAllPosts(int pageNumber, int pageSize)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/posts/GetAllPosts?sort=desc&pageNumber={0}&pageSize={1}", pageNumber, pageSize));
            return JsonConvert.DeserializeObject<List<Post>>(response);
        }

        public static async Task<List<UsersGetList>> GetAllUsers()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/users/GetAllUsers"));
            return JsonConvert.DeserializeObject<List<UsersGetList>>(response);
        }
        public static async Task<List<Comment>> GetPostComments(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/posts/GetPostComments/" + id));
            return JsonConvert.DeserializeObject<List<Comment>>(response);
        }
        public static async Task<List<MessageModel>> GetConversationMessages(int conversationId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("acessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/Chat/GetConversationMessages/" + conversationId));
            return JsonConvert.DeserializeObject<List<MessageModel>>(response);
        }

        public static async Task<int>  VerifyIfConversationExists(int userid2)
        {
            await TokenValidator.CheckTokenValidity();
            var id = Preferences.Get("userId", 0);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var conversation = new ConversationAdd()
            {
                User1Id = id,
                User2Id = userid2
            };
            var jsonConversation = JsonConvert.SerializeObject(conversation);
            var content = new StringContent(jsonConversation, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/chat/VerifyIfConversationExists/", content);
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }
        public static async Task<bool> AddPostComment(string description, int postId)
        {
            var id = Preferences.Get("userId", 0);
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("acessToken", string.Empty));
            var comment = new CommentAdd()
            {
                Description = description,
                UserId = id,
                PostId = postId
            };
            var jsonComment = JsonConvert.SerializeObject(comment);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/posts/addcomment/", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<bool> CreateConversation(int userid2)
        {
            await TokenValidator.CheckTokenValidity();
            var id = Preferences.Get("userId", 0);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var conversation = new ConversationAdd()
            {
                User1Id = id,
                User2Id = userid2
            };
            var jsonComment = JsonConvert.SerializeObject(conversation);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Chat/CreateConversation/", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<bool> CreateMessage(int conversationId,string receiverUserName,string messageText)
        {
            await TokenValidator.CheckTokenValidity();
            var id = Preferences.Get("userId", 0);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var message = new MessageModel
            {
                ConversationId = conversationId,
                SenderUserId = id,
                ReceiverUserName = receiverUserName,
                Messagee = messageText
            };
            var jsonMessage = JsonConvert.SerializeObject(message);
            var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Chat/CreateMessage/", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<UserLogged> GetUserLoggedInfo()
        {
            var id = Preferences.Get("userId", 0);
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("acessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/users/GetLoggedUserInfo/" + id));
            return JsonConvert.DeserializeObject<UserLogged>(response);
        }
        public static async Task<UserLogged> GetUserInfo(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("acessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/users/GetLoggedUserInfo/" + id));
            return JsonConvert.DeserializeObject<UserLogged>(response);
        }
        public static class TokenValidator
        {
            public static async Task CheckTokenValidity()
            {
                var expirationTime = Preferences.Get("tokenExpiration", 0);
                Preferences.Set("currentTime", UnixTime.GetCurrentTime());
                var currentTime = Preferences.Get("currentTime", 0);
                if (expirationTime < currentTime)
                {
                    var email = Preferences.Get("UserName", string.Empty);
                    var password = Preferences.Get("password", string.Empty);
                    string tokenFirebase = Preferences.Get("TokenFirebase", string.Empty);
                    await Login(email, password);
                }
            }
        }
    }
}
