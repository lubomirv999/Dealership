using Newtonsoft.Json.Linq;
using System.IO;

namespace Dealership.Models
{
    public static class CredentialsBuilder
    {
        public static string ConnectionString { get; private set; }

        public static string Host { get; private set; }

        public static string EmailId { get; private set; }

        public static string EmailPassword { get; private set; }

        public static string AdminUsername { get; private set; }

        public static string AdminEmail { get; private set; }

        public static string AdminPassword { get; private set; }

        public static string AdminRole { get; private set; }

        public static string ModeratorRole { get; private set; }

        public static void SetCredentials()
        {
            string filePath = "credentials.json";
            string parsed = string.Empty;

            using (StreamReader reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                var jsonObj = JObject.Parse(json);

                ConnectionString = jsonObj["ConnectionString"].ToString();
                Host = jsonObj["Host"].ToString();
                EmailId = jsonObj["EmailId"].ToString();
                EmailPassword = jsonObj["EmailPassword"].ToString();
                AdminUsername = jsonObj["AdminUsername"].ToString();
                AdminEmail = jsonObj["AdminEmail"].ToString();
                AdminPassword = jsonObj["AdminPassword"].ToString();
                AdminRole = jsonObj["AdminRole"].ToString();
                ModeratorRole = jsonObj["ModeratorRole"].ToString();
            }
        }
    }
}
