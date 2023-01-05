using IMF.Gateway;
using IMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;


namespace IMF.Manager
{
    public class PositionManager
    {
        AccountGateway _accountGateway = new AccountGateway();

        public bool CreateNewAccount(int clientId, float Cash)
        {
            return _accountGateway.CreateNewAccount(clientId, Cash);
        }

        public bool AddPosition(Position position)
        {
            return _accountGateway.AddPosition(position);
        }
        public bool UpdateSODPosition(int sod, int clientId,string sedol)
        {
            return _accountGateway.UpdateSODPosition(sod, clientId, sedol);
        }
        public bool UpdateAccountCash(int clientId, int cash)
        {
            return _accountGateway.UpdateAccountCash(clientId,cash);
        }
        public bool Buy(int clientId, string sedol, float buy)
        {
            return _accountGateway.Buy(clientId, sedol, buy);
        }
        public bool Sell(int clientId, string sedol, float sell)
        {
            return _accountGateway.Sell(clientId, sedol, sell);
        }
        public bool ReleaseReserveFromSod(int clientId, string sedol, float release)
        {
            return _accountGateway.ReleaseReserveFromSod(clientId, sedol, release);
        }
        public bool MoveReserveToExecuted(int clientId, string sedol, float reserveToExecuted)
        {
            return _accountGateway.MoveReserveToExecuted(clientId, sedol, reserveToExecuted);
        }
        public List<Account> GetAllAccounts()
        {
            return _accountGateway.GetAllAccounts();
        }
        public List<Position> GetAllPostions()
        {
            return _accountGateway.GetAllPositions();
        }
        public IDictionary<string, float>? CheckAcountPositionStatus(int clientId,string sedol)
        {
            return _accountGateway.CheckAcountPositionStatus(clientId, sedol);
        }
        public IDictionary<string, float>? CheckAccountStatus(int clientId)
        {
            return _accountGateway.CheckAccountStatus(clientId);
        }
        public bool Delete(int clientId)
        {
            return _accountGateway.Delete(clientId);
        }
        public Symbol SymbolMaker(string sedol, string exchange, string instrumentType, int id)
        {
            /// <summary>
            /// This Function create new symbol
            /// </summary>

            Symbol symbol = new Symbol();
            symbol.Sedol= sedol;
            symbol.Exchange= exchange; 
            symbol.InstrumentType= instrumentType;
            symbol.Id = id;
            return symbol;
        }
        public Position PositionMaker(
                                    int sod,
                                    int reserved,
                                    int executed, 
                                    int clientid, 
                                    Symbol symbol
                                    )
        {
            /// <summary>
            /// This Function create Position with a symbol
            /// </summary>
      
            Position position = new Position();
            position.Sod= sod;
            position.Reserved= reserved;
            position.Executed= executed;
            position.AccountId= clientid;
            position.Symobl = symbol;
            return position;
        }
    }
}
