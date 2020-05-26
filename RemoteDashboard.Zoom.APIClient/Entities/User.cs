using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDashboard.Zoom.APIClient
{
    public class User
    {
        public string id { get { return r_id; } set { r_id=value; } }
        private string r_id;

        public string first_name { get { return r_first_name; } set { r_first_name=value; } }
        private string r_first_name;

        public string last_name { get {return r_last_name; } set { value = r_last_name; } }
        private string r_last_name;

        public string email { get { return r_email; } set { value = r_email; } }
        private string r_email;

        public int type { get { return r_type; } set {value = r_type; } }
        private int r_type;

        public int pmi { get { return r_pmi; } set { value = r_pmi; } }
        private int r_pmi;

        public string timezone { get { return r_timezone; } set {value=r_timezone; } }
        private string r_timezone;

        public int verified { get { return r_verified; } set { value = r_verified; } }
        private int r_verified;

        public string dept { get { return r_dept; } set { value = r_dept; } }
        private string r_dept;

        public DateTime created_at { get { return r_created_at; } set { value = r_created_at; } }
        private DateTime r_created_at;

        public DateTime last_login_time { get { return r_last_login_time; } set { value = r_last_login_time; } }
        private DateTime r_last_login_time;

        public string last_client_version { get { return r_last_client_version; } set { value = r_last_client_version; } }
        private string r_last_client_version;

        public string language { get { return r_language; } set { value = r_language; } }
        private string r_language;

        public string phone_number { get { return r_phone_number; } set { value = r_phone_number; } }
        private string r_phone_number;

        public string status { get { return r_status; } set { value = r_status; } }
        private string r_status;

        public void Save()
        {
           DataFactory.DataFactory.Save(this);
        }
    }
}
