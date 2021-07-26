using InstagramClone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public static async Task<bool> RegisterUser(string name,string lastName,string secondLastName,string email,string password,string phoneNumber,string tokenFirebase)
        {
            var user = new User()
            {
                Name=name,
                LastName=lastName,
                SecondLastName=secondLastName,
                Email=email,
                Password=password,
                PhoneNumber=phoneNumber,
                TokenFirebase=tokenFirebase
            };
            var httpClient = new HttpClient();
            var jsonRegister = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var ApiResponse = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/Register", content);
            if (!ApiResponse.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<bool> Login(string email, string password, string tokenfirebase)
        {
            var login = new User()
            {
                Email = email,
                Password = password,
                TokenFirebase = tokenfirebase
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
        public static async Task<List<Post>> GetAllPosts(int pageNumber,int pageSize)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/posts/GetAllPosts?sort=asc&pageNumber={0}&pageSize={1}", pageNumber, pageSize));
            return JsonConvert.DeserializeObject<List<Post>>(response);
        }
        public static async Task<List<Comment>> GetPostComments(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + string.Format("api/posts/GetPostComments/" + id));
            return JsonConvert.DeserializeObject<List<Comment>>(response);
        }
        public static async Task<bool> AddPostComment(string description,int postId)
        {
            var id = Preferences.Get("userId", 3);
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("acessToken", string.Empty));
            var comment = new CommentAdd()
            {
                Description = description,
                UserId = id,
                PostId=postId
            };
            var jsonComment = JsonConvert.SerializeObject(comment);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/posts/addcomment/", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<UserLogged> GetUserLoggedInfo()
        {
            var id = Preferences.Get("userId", 3);
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
                    var email = Preferences.Get("email", string.Empty);
                    var password = Preferences.Get("password", string.Empty);
                    string tokenFirebase = Preferences.Get("TokenFirebase", string.Empty);
                    await ApiService.Login(email, password, tokenFirebase);
                }
            }
        }
    }
}
