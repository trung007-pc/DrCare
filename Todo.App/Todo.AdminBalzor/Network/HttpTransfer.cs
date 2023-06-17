using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using WebClient.Exceptions;

namespace Todo.AdminBlazor.Network;

public static class HttpTransfer
{
    
    public static HttpClient HttpClientAsync(this IHttpClientFactory factory,string name, string Token = "")
    {
        var client = factory.CreateClient(name);
        if (!string.IsNullOrEmpty(Token)) client.DefaultRequestHeaders.Add("authorization", "bearer " + Token);
        return client;
    }
    
      public static async Task<T> GetAPIAsync<T>(this HttpClient httpClient,[Required] string URL)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage = await httpClient.GetAsync(URL);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }
        
        public static async Task<T> PostAPIAsync<T>(this HttpClient httpClient,[Required] string URL, dynamic input)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();


            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

            httpResponseMessage = await httpClient.PostAsync(URL, content);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }

        public static async Task<T> PatchAPIAsync<T>(this HttpClient httpClient,[Required] string URL, dynamic input)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();


            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

            httpResponseMessage = await httpClient.PatchAsync(URL, content);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }


        // public static async Task<T> PostAPIWithFileAsync<T>(this HttpClient httpClient,[Required] string URL, IBrowserFile file)
        // {
        //     HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        //
        //     using (var ms = new MemoryStream())
        //     {
        //         await file.OpenReadStream(UploadLimit).CopyToAsync(ms);
        //
        //         ms.Seek(0, SeekOrigin.Begin);
        //         using var content = new MultipartFormDataContent
        //         {
        //             {new StreamContent(ms), "file", file.Name}
        //         };
        //
        //         httpResponseMessage = await httpClient.PostAsync(URL, content);
        //         return await ReturnApiResponse<T>(httpResponseMessage);
        //     }
        // }
        //
        // public static async Task<T> PostAPIWithMultipleFileAsync<T>(this HttpClient httpClient,[Required] string URL, List<IBrowserFile> files)
        // {
        //     HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        //
        //     var streams = new List<MemoryStream>();
        //
        //     var ms = new MemoryStream();
        //     using (var content = new MultipartFormDataContent())
        //     {
        //         foreach (var file in files)
        //         {
        //             await file.OpenReadStream(UploadLimit).CopyToAsync(ms);
        //             ms.Seek(0, SeekOrigin.Begin);
        //             content.Add(new StreamContent(ms), $"files", file.Name);
        //             streams.Add(ms);
        //             ms = new MemoryStream();
        //
        //         }
        //
        //         httpResponseMessage = await httpClient.PostAsync(URL, content);
        //         foreach (var item in streams)
        //         {
        //             item.Close();
        //         }
        //
        //         return await ReturnApiResponse<T>(httpResponseMessage);
        //     }
        // }


        public static async Task<T> PutAPIAsync<T>(this HttpClient httpClient,[Required] string URL, dynamic input)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            httpResponseMessage = await httpClient.PutAsync(URL, content);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }

        public static async Task<T> DeleteAPIAsync<T>(this HttpClient httpClient,[Required] string URL)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage = await httpClient.DeleteAsync(URL);
            return await ReturnApiResponse<T>(httpResponseMessage);
        }


        private static async Task<T> ReturnApiResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string? jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync() ?? null;
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Not Found");
            }

            

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("");
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequests("Too many request");
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.Conflict)
            {
                string? jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync() ?? null;
                var response = JsonConvert.DeserializeObject<ResponseApi>(jsonResponse);

                throw new ConflictException(response.message);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.BadGateway)
            {
                throw new DbConnectionException("connection-error");
            }

            
            if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                string? jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync() ?? null;
                var response = JsonConvert.DeserializeObject<ResponseApi>(jsonResponse);
                throw new BadRequestException(response.message);
            }
            if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ServerErrorException("Server-Error");
            }
            

            return default;
        }
        
        public class ResponseApi
        {
            public string message { get; set; }
            public int status { get; set; }
        }
}