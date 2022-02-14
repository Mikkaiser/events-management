using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    static class requester
    {
        public async static Task<T> get<T>(string endpoint)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"http://192.168.1.38:8080/inscricao-service/api/{endpoint}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }


        public async static Task<object> put(string endpoint, object obj)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await client.PutAsync($"http://192.168.1.38:8080/inscricao-service/api/{endpoint}", content);
   

            return response;
        }

        public async static Task<object> post(string endpoint, object obj)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await client.PostAsync($"http://192.168.1.38:8080/inscricao-service/api/{endpoint}", content);


            return response;
        }

        public async static Task<object> delete(string endpoint)
        {
            HttpClient client = new HttpClient();

            var response = await client.DeleteAsync($"http://192.168.1.38:8080/inscricao-service/api/{endpoint}");

            return response;
        }
    } 
}
