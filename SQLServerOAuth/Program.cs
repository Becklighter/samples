using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Data.SqlClient;


namespace SQLServerOAuth
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            string appId = "<app id>";

            string connection = $"RunAs=App;AppId={appId};TenantId={AppSettings.TENANT_ID};AppKey={AppSettings.SECRET}";

            Environment.SetEnvironmentVariable("AzureServicesAuthConnectionString", connection);

            //Not use, but can be used for other libraries
            //var credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(appId, AppSettings.SECRET);

            //MAC Instance
            string connectionString = @"Server=tcp:<AzureSQL Name>.database.windows.net,1433;Initial Catalog=<db name>;";

            var ap = new AzureServiceTokenProvider(); 
            SqlConnection conn = new SqlConnection(connectionString);

            //Get token for resource
            var accessToken  = await ap.GetAccessTokenAsync("https://database.windows.net/");

            //connection token 
            conn.AccessToken = accessToken;

            SqlCommand command = new SqlCommand("select * from Employee", conn);
            command.Connection.Open();
            var rdr = command.ExecuteReader();

            // print the CustomerID of each record
            while (rdr.Read())
            {
                Console.WriteLine($"{rdr[0]} {rdr[1]}");
            }
            conn.Close();
            Console.ReadLine();

        }
    }
}
