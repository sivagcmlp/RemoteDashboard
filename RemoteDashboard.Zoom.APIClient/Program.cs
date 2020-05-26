using RestSharp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemoteDashboard.Zoom.APIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAllUesrs();
        }

        private static void GetAllUesrs()
        {
            var client = new RestClient("https://api.zoom.us/v2/report/users?from=2020-05-12&to=2020-05-16&page_size=100&type=active")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOm51bGwsImlzcyI6IkRTWGVVQzdqUjQtX2QwS0tkY3hiTlEiLCJleHAiOjE1OTA0NDU0MjQsImlhdCI6MTU4OTg0MDYyNH0.zn_QAeGEmM5VkTNXm8Xkcn-ECAQ2SDrllQ3HsMIt64w");
            request.AddHeader("Cookie", "cred=59BB2C9E354D348A21E0D37EC3B64DA4");
            IRestResponse<UserRoot> response = client.Execute<UserRoot>(request);
            //var resr = JsonConvert.DeserializeObject<UserRoot>(response.Content);
            if (response.Data.users.Count > 0)
            {
                User obj = new User
                {
                    id = response.Data.users[0].id,
                    first_name = response.Data.users[0].first_name,
                    last_name = response.Data.users[0].last_name,
                    email = response.Data.users[0].email,
                    pmi = response.Data.users[0].pmi,
                    type = response.Data.users[0].type,
                    dept = response.Data.users[0].dept,
                    verified = response.Data.users[0].verified,
                    last_login_time = response.Data.users[0].last_login_time,
                    last_client_version = response.Data.users[0].last_client_version,
                    status = response.Data.users[0].status,
                    language = response.Data.users[0].language,
                    created_at = response.Data.users[0].created_at,
                    phone_number = response.Data.users[0].phone_number
                };
                obj.Save();
            }
            Console.WriteLine(response.Content);
            Console.ReadLine();

        }
    }
}
