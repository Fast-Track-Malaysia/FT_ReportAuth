using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using FT_SpReport.CoreBusiness.Helpers;
using FT_SpReport.CoreBusiness.Models;
using System.IO;

namespace FT_SpReport.CoreBusiness.Services
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private ILocalStorageService _localStorageService;

        public HttpService(
            HttpClient httpClient,
            ILocalStorageService localStorageService
        )
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task Login(string username, string password)
        {
            try
            {
                /*
                Uri uri = new Uri(_httpClient.BaseAddress + "login");
                LoginModel userlogin = new LoginModel() { UserName = username, Password = password };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(userlogin);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, StaticValues.APP_JSON);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.APP_JSON));
                _httpClient.DefaultRequestHeaders.Add(StaticValues.APP_CORS, "*");
                HttpResponseMessage response = await _httpClient.PostAsync(uri, stringContent);
                */
                var requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(_httpClient.BaseAddress + "login"),
                    Content =
                    JsonContent.Create(new LoginModel
                    {
                        UserName = username,
                        Password = password
                    })
                };
                //requestMessage.Headers.Add(StaticValues.APP_CORS, "*");
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                bool success = response.IsSuccessStatusCode;
                if (!success) return;

                string bearer = await response.Content.ReadAsStringAsync();
                await _localStorageService.SetItem(StaticValues.BEARER, bearer);
                Console.WriteLine("Logined");
                /*
                requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("GET"),
                    RequestUri = new Uri(_httpClient.BaseAddress + "api/systemusers/" + username)
                };
                requestMessage.Headers.Add(StaticValues.APP_CORS, "*");
                requestMessage.Headers.Add(StaticValues.Authorization, StaticValues.BEARER + " " + bearer);
                //requestMessage.Headers.Add(StaticValues.COOKIE, cookie); // Blazor WASM do not allowed cookie
                response = await _httpClient.SendAsync(requestMessage);
                if (!success) return;

                string content = await response.Content.ReadAsStringAsync();
                UserLogin usr = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLogin>(content);
                usr.Bearer = bearer;
                */
                LocalStorageUserModel usr = await Get<LocalStorageUserModel>("api/systemusers/" + username);
                usr.AuthData = $"{username}:{password}".EncodeBase64();
                await _localStorageService.SetItem(StaticValues.USERLS, usr);
                Console.WriteLine(usr.UserName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }
        // helper methods

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            try
            {
                string bearer = await _localStorageService.GetItem<string>(StaticValues.BEARER);
                if (!string.IsNullOrWhiteSpace(bearer))
                    request.Headers.Add(StaticValues.Authorization, StaticValues.BEARER + " " + bearer);

                //request.Headers.Add(StaticValues.APP_CORS, "*");

                // add basic auth header if user is logged in and request is to the api url

                var user = await _localStorageService.GetItem<LocalStorageUserModel>(StaticValues.USERLS);
                if (user != null)
                {
                    var isApiUrl = !request.RequestUri.IsAbsoluteUri;
                    if (user != null && isApiUrl)
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", user.AuthData);
                }
            }
            catch
            { }

            var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //_navigationManager.NavigateTo("logout");
                return default;
            }

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }


        public async Task<object> PostReturnStream(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequestReturnStream(request);
        }

        private async Task<object> sendRequestReturnStream(HttpRequestMessage request)
        {
            string bearer = await _localStorageService.GetItem<string>(StaticValues.BEARER);
            if (!string.IsNullOrWhiteSpace(bearer))
                request.Headers.Add(StaticValues.Authorization, StaticValues.BEARER + " " + bearer);
            //request.Headers.Add(StaticValues.APP_CORS, "*");

            // add basic auth header if user is logged in and request is to the api url
            var user = await _localStorageService.GetItem<LocalStorageUserModel>(StaticValues.USERLS);
            if (user != null)
            {
                var isApiUrl = !request.RequestUri.IsAbsoluteUri;
                if (user != null && isApiUrl)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", user.AuthData);
            }
            var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //_navigationManager.NavigateTo("logout");
                return default;
            }

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
            return await response.Content.ReadAsStreamAsync();
            //var responseStream = await response.Content.ReadAsStreamAsync();
            //try
            //{
            //    return await JsonSerializer.DeserializeAsync<T>(responseStream);
            //}
            //catch (Exception ex)
            //{
            //    return default;
            //}
        }
    }
}
