namespace Sales.Services
{
    
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Common.Models;
    using Sales.Helper;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurOnInternet,
                };
            }

            //var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com",80,5000);
            bool isReachable = await CrossConnectivity.Current.IsReachable("google.com",5000);
            if (!isReachable)
            {
                //Todo:https://www.youtube.com/watch?v=rMtDVQkGBVc&list=PLuEZQoW9bRnRnzwx4z1kzoY2Pt2nve6L_&index=10

                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };

            }

            return new Response
            {
                IsSuccess = true,
            };

        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlBase);

                string ulr = $"{prefix}{controller}";

                HttpResponseMessage response = await cliente.GetAsync(ulr);


                string answer = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }
    }
}
