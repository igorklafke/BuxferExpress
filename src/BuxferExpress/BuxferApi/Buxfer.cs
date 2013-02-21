using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BuxferExpress.BuxferApi
{
    public class Buxfer
    {
        string token;
        string baseLoginUrl = "https://www.buxfer.com/api/login.json?userid={0}&password={1}";
        string baseCmdUrl = "https://www.buxfer.com/api/{0}.json?token={1}";

        public bool Login(string username, string password)
        {
            BuxferOutput output = DoCall(string.Format(baseLoginUrl, username, password));
            if (output.ResponseOk())
            {
                token = output.response.token;
                return true;
            }
            else
                return false;
        }

        public List<Account> GetAccounts()
        {
            string uri = string.Format(baseCmdUrl, "accounts", token);
            BuxferOutput output = DoCall(uri);
            if (output.ResponseOk())
                return output.response.accounts.ToList();
            else
                return new List<Account>();
        }

        public void AddTransaction(string text)
        {
            string format = "sms";
            string url = string.Format(baseCmdUrl, "add_transaction", token) + string.Format("&format={0}&text={1}", format, text);
            BuxferOutput output = DoCall(url);
            if (output.ResponseOk())
            {
                string teste = output.response.parseStatus;
            }

        }

        public void AddTransaction(string description, string amount, DateTime date, string accountName = "", string tags = "")
        {
            //Todo: Tratar melhor os espaços
            string text = description.Trim().Replace(" ", "%20") + "%20" + amount;

            if (!string.IsNullOrEmpty(accountName))
                text += string.Format("%20acct:{0}", accountName.Replace(" ", "%20"));
            if (!string.IsNullOrEmpty(tags))
                text += string.Format("%20tags:{0}", tags.Replace(" ", "%20"));

            text += string.Format("%20date:{0}", date.ToString("yyyy-MM-dd"));

            AddTransaction(text);

        }

        private BuxferOutput DoCall(string uri)
        {
            WebClient client = new WebClient();

            string result = client.DownloadString(new Uri(uri));

            BuxferOutput output = Deserialize<BuxferOutput>(result);

            return output;
        }

        private static T Deserialize<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(memoryStream);
                return obj;
            }
        }
    }
}
