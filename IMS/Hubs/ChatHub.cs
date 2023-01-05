using IMF.Gateway;
using IMF.Manager;
using IMF.Models;
using log4net;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static SQLite.SQLite3;

namespace IMF.Hubs
{
    public class ChatHub : Hub
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Program));

        public async Task SendMessage(string userName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, "API works fine");
        }
        public IDictionary<string, float>? CheckAccountStatus(string clientIdMsg)
        {
            int clientId = Int32.Parse(clientIdMsg);
            PositionManager PositionMangber = new PositionManager();
            IDictionary<string, float>? status = PositionMangber.CheckAccountStatus(clientId);
            return status;
        }
        public string Buy(string clientIdMsg,
                            string sedol,
                            string buyMsg)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            float buy = float.Parse(buyMsg);
            PositionManager positionManager = new PositionManager();
            bool result = positionManager.Buy(clientId, sedol, buy);

            if (result == false)
            {
                _logger.ErrorFormat("Unable to find clientId:{0}", clientId);
                return "not enough cash to buy";
            }
            else
            {
                _logger.InfoFormat("Buy request sucess");
                return "Buy request sucess";
            }
        }

        public string AddPosition(string clientIdMsg,
                    string sedol,
                    string exchange,
                    string symbolIdMsg,
                    string description)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            int symbolId = Int32.Parse(symbolIdMsg);
            PositionManager positionManager = new PositionManager();
            Symbol symbol = positionManager.SymbolMaker(sedol, exchange, description, symbolId);
            Position position = positionManager.PositionMaker(0, 0, 0, clientId, symbol);
            bool result  = positionManager.AddPosition(position);
            if (result == false)
            {
                string msg = "fail on add position";
                _logger.ErrorFormat(msg);
                return msg;
            }
            else
            {
                string msg = "add position sucess";
                _logger.InfoFormat(msg);
                return msg;
            }
        }

        public string Sell(string clientIdMsg,
                    string sedol,
                    string sellMsg)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            float sell = float.Parse(sellMsg);
            PositionManager positionManager = new PositionManager();
            bool result =  positionManager.Sell(clientId, sedol, sell);
            if (result == false)
            {
                string msg = "not enough SOD/Reserve to sell";
                _logger.ErrorFormat(msg);
                return msg;
            }
            else
            {
                string msg = "Sell request sucess";
                _logger.InfoFormat(msg);
                return msg;
            }
        }
        public string UpdateAccountCash(string clientIdMsg,
            string cashMsg)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            int cash = Int32.Parse(cashMsg);
            PositionManager positionManager = new PositionManager();
            bool result = positionManager.UpdateAccountCash(clientId, cash);
            if (result == false)
            {
                string msg = "cannot find client id";
                _logger.ErrorFormat(msg);
                return msg;
            }
            else
            {
                string msg = "Account cash balance is updated";
                _logger.InfoFormat(msg);
                return msg;
            }
        }
        public string UpdateSODPosition(string clientIdMsg,
                    string sedol,
                    string sodMsg)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            int sod = Int32.Parse(sodMsg);
            PositionManager positionManager = new PositionManager();
            bool result = positionManager.UpdateSODPosition(sod, clientId,sedol);
            if (result == false)
            {
                string msg = "fail to find symobl in client account/position";
                _logger.ErrorFormat(msg);
                return msg;
            }
            else
            {
                string msg = "Sod position updated";
                _logger.InfoFormat(msg);
                return msg;
            }
        }

        public bool CreateNewAccount(string clientIdMsg,
            string cashMsg)
        {
            // #TODO 1. Need to figure out how signalR client side send int/float type var instead string
            // #TODO 2. The hub API better off to return an wrapped object contains sucess/fail message. account/position/symobl attributes

            int clientId = Int32.Parse(clientIdMsg);
            float cash = float.Parse(cashMsg);
            PositionManager positionManager = new PositionManager();
            return positionManager.CreateNewAccount(clientId, cash);
        }

        public bool Delete(string clientIdMsg)
        {
            int clientId = Int32.Parse(clientIdMsg);
            PositionManager positionManager = new PositionManager();
            return positionManager.Delete(clientId);
        }

        public bool GetAllAccount()
        {
            //function already available in the positionManager interface, but not implment in the signalR hub
            PositionManager positionManager = new PositionManager();
            positionManager.GetAllAccounts();
            throw new NotImplementedException();
        }
        public bool MoveReserveToExecuted()
        {
            //function already available in the positionManager interface, but not implment in the signalR hub
            PositionManager positionManager = new PositionManager();
            positionManager.MoveReserveToExecuted(11234,"",0);
            throw new NotImplementedException();
        }

        public bool ReleaseReserveFromSod()
        {
            //function already available in the positionManager interface, but not implment in the signalR hub
            PositionManager positionManager = new PositionManager();
            positionManager.ReleaseReserveFromSod(0, "", 0);
            throw new NotImplementedException();
        }


    }
}
