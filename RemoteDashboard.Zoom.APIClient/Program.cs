using RestSharp;
using System;
using RemoteDashboard.Models.Models;
using Newtonsoft.Json;

namespace RemoteDashboard.Zoom.APIClient
{
    class Program
    {
        static string connString = "";
        static void Main(string[] args)
        {
            GetAllUesrs();
        }

        private static void GetAllUesrs()
        {
            var client = new RestClient("https://api.zoom.us/v2/users?page_size=1")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOm51bGwsImlzcyI6IkRTWGVVQzdqUjQtX2QwS0tkY3hiTlEiLCJleHAiOjE1OTM1MjIwMDAsImlhdCI6MTU5MDUzMjg1OX0.MYRwBUqxuIufuRZlMzSVIO76hYeavNds9xwotPwsHFg");
            request.AddHeader("Cookie", "cred=59BB2C9E354D348A21E0D37EC3B64DA4");
           // IRestResponse response = client.Execute(request);
            IRestResponse<UserRoot> response = client.Execute<UserRoot>(request);
            //var resr = JsonConvert.DeserializeObject<UserRoot>(response.Content);
            if (response.Data != null && response.Data.users.Count > 0)
            {
                User objUser = new User
                {
                    Id = response.Data.users[0].id,
                    FirstName = response.Data.users[0].first_name,
                    LastName = response.Data.users[0].last_name,
                    Email = response.Data.users[0].email,
                    PMI = response.Data.users[0].pmi,
                    Type = response.Data.users[0].type,
                    Department = response.Data.users[0].dept,
                    Verified = response.Data.users[0].verified,
                    LastLoginTime = response.Data.users[0].last_login_time,
                    LastClientVersion = response.Data.users[0].last_client_version,
                    Status = response.Data.users[0].status,
                    Language = response.Data.users[0].language,
                    Created = response.Data.users[0].created_at,
                    PhoneNumber = response.Data.users[0].phone_number
                };

                var userRepo = new DataAccess.Repository<User>(connString);
                //TODO: Insert query to be passed once the DB is ready
                string insertUserQry = "";

                userRepo.Add(objUser, insertUserQry);
            }

        }
    }
}
