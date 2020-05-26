using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteDashboard.Zoom.APIClient
{
    public class DataModel
    {
        
    }

    //This is the main object - jSon Object
    public class RootObject
    {
        public Status status { get; set; }
        public List<Datum> data { get; set; }
        public pagination pagination { get; set; }
    }

    public class UserRoot
    {
        public int page_count { get; set; }
        public int page_number { get; set; }
        public int page_size { get; set; }
        public int total_records { get; set; }
        public List<User> users { get; set; }
    }

    public class Status //This is the jSON status
    {
        public bool error { get; set; }
        public int code { get; set; }
        public string type { get; set; }
        public string message { get; set; }
    }

    public class Datum //This is the actual data that is returned
    {
        public string access_token { get; set; }
        public string created_at { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int account_id { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        public custom_attributes custom_attributes { get; set; }

    }

    public class ZoomUser
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int type { get; set; }
        public int pmi { get; set; }
        public string timezone { get; set; }
        public int verified { get; set; }
        public string dept { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_login_time { get; set; }
        public string last_client_version { get; set; }
        public string language { get; set; }
        public string phone_number { get; set; }
        public string status { get; set; }
    }

    public class pagination //This is for pagination of record sets that are greater than 50 records
    {
        public string next_link { get; set; }
        public string previous_link { get; set; }
    }

    //These will be your custom attributes, where you'd substitute customAttribute1, etc with your real custom attribute names
    public class custom_attributes
    {
        public string customAttribute1 { get; set; }
        public string customAttribute2 { get; set; }
    }
}
