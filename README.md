# IMS
IMS Mock APP

1. Local vscode run: Launch Program.cs to start IMS signalR WebService
2. Folder Model contain table attributes and attribute context
	a. ApplicationDbContext.cs 
		i.  DB path is harded coded in line:optionsBuilder.UseSqlite(@"Data Source=D:\git\IMS\DB\IMS.db");
		ii. Can add hard coded path into an app.config.
3. Folder Hub: ChatHub class use signalR to make connection between service and client
4. Folder Gateway: core inventory account/postion inv management logics
5. Migrations, util function create new db tables with correspond attributes
6. Folder DB, SQLite IMF.Db file, I used SOLite Expert Personal 5.4 to monitor attribute change
7. IMFTest for unit test include following function
	a. create account
	b. buy/sell, check sod avaiblity for sell/cash avaialbility for buy
	c. move cash reserve to SOD.
	d. check account status.
	e. delete account
	f. update sod postion for selling and check sod availbilituy
8. Folder GateWay, core sqlite logic for all the functions
9. Folder Manager, interface class for Gateway
10.Project IMF client do following:
	b. create hub connection builder
	c. sample codes in Program.cs to send requests
		i.  CreateNewAccount
		ii. AddPosition
		iii.Buy
		iv. Sell
		v. Few method are NotImplemented in the hub interface, but available in PostionManager.cs and AccountGateWay.cs
11. Startup.cs, class handles signal R service startup
12. Folder DepndencyInjection, generic IOC classes handles Service/App register/conrtainer/descrpitor/collection
13. Release
	a. Windonws: 
		i. build artifact, run command in terminal: dotnet publish -c release --self-contained --runtime win-x64 --framework netcoreapp6.0
		ii.Run command, click IMF.exe in publish folder --> D:\git\IMS\IMS\bin\release\netcoreapp6.0\win-x64\publish\IMF.exe
	b. Linux: 
		i.   build artifact run command in terminal:  dotnet publish -c release --self-contained --runtime linux-x64 --framework netcoreapp6.0
		ii.  I dont have ubuntu installed in my pc, but an ideal sample .sh would be below
			 #!/bin/bash
			 dotnet run --project /someLinuxDir/IMS/IMS/bin/release/netcoreapp6.0/linux-x64/IMF.dll "$1"

14. Client Side Request sample also attached inseperate project.
i.e：client/positon/symobl attributes.
Sample:

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