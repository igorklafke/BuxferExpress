using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BuxferExpress.BuxferApi
{
    public class BuxferOutput
    {
        public Response response { get; set; }

        public bool ResponseOk()
        {
            return this.response.status == "OK";
        }
    }

    public class Response
    {
        public string status { get; set; }
        public string token { get; set; }

        public IEnumerable<Account> accounts { get; set; }

        public string parseStatus { get; set; }
    }

    [DataContract]
    public class Account
    {
        [DataMember(Name = "key-account")]
        public KeyAccount key_account { get; set; }
    }

    public class KeyAccount
    {
        public string id { get; set; }
        public string name { get; set; }
        public string bank { get; set; }
        public double balance { get; set; }
    }
}
