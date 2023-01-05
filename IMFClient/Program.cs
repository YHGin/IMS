using Microsoft.AspNetCore.SignalR.Client;

namespace IMFClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chatHub")
                .Build();
            connection.StartAsync().Wait();

            string clientId = "98765";
            string sedol = "A00012";
            string exchange = "SS";
            string symoblId = "623004";
            string instrumentType = "Equity";
            var result0 = connection.InvokeAsync<bool>("CreateNewAccount", clientId, "20000").Result;
            var result05 = connection.InvokeAsync<string>("UpdateAccountCash", clientId, "10000").Result;
            var result1 = connection.InvokeAsync<string>("AddPosition", clientId, sedol, exchange, symoblId, instrumentType).Result;
            var result2 = connection.InvokeAsync<string>("Buy", clientId, sedol, "5200").Result;
            var result25 = connection.InvokeAsync<string>("UpdateSODPosition", clientId, sedol, "2000").Result;
            var result3 = connection.InvokeAsync<string>("Sell", clientId, sedol, "1000").Result;
            var result4 = connection.InvokeAsync<IDictionary<string, float>>("CheckAccountStatus", clientId).Result;


        }
    }
}