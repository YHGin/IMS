using IMF.Models;
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IMF.Gateway
{
    public class AccountGateway
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        private static ILog _logger = LogManager.GetLogger(typeof(Program));
        public AccountGateway()
        {

        }

        public bool CreateNewAccount(int clientId,float Cash)
        {
            /// <summary>
            /// This Function create new account with a dummy position, 
            /// #TODO, Can add with Position to create both account and positions in one go
            /// </summary>
     
            var account = _dbContext.Accounts.FirstOrDefault(c => c.Id == clientId);
            if (account != null)
            {
                return false;
            }
            else
            {
                Account NewAccount = new Account();
                Position dummyPosition = new Position();
                Symbol dummySymbol = new Symbol();
                List<Position> dummyPositions = new List<Position>();
                NewAccount.Id = clientId;
                NewAccount.Cash = Cash;
                _dbContext.Accounts.Add(NewAccount);
                return _dbContext.SaveChanges() > 0;
            }          

        }

        public bool AddPosition(Position position)

        {
            /// <summary>
            /// This Function Add position if account exist & postion not exist. Update position if account exist and position exist
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == position.AccountId);
            if (account == null)
            {
                return false;
            }
            var oldPosition = _dbContext.Positions.FirstOrDefault(p => p.AccountId == position.AccountId && p.Symobl.Sedol == position.Symobl.Sedol);
            if (oldPosition == null)
            {
                _dbContext.Positions.Add(position);
                return _dbContext.SaveChanges() > 0;
            }
            else
            {
                oldPosition.Reserved = position.Reserved;
                oldPosition.Sod = position.Sod;
                oldPosition.Executed= position.Executed;
                _dbContext.Positions.Update(oldPosition);
                return _dbContext.SaveChanges() > 0;
            }
        }

        public bool UpdateSODPosition(int newSod, int clientId, string sedol)
        {
            /// <summary>
            /// This Function overwirete existing position SOD
            /// </summary>
   
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol== sedol);
            if (position == null)
            {
                return false;
            }
            else
            {
                position.Sod = newSod;
                _dbContext.Positions.Update(position);
                return _dbContext.SaveChanges() > 0;
            }
        }
        public bool UpdateAccountCash(int clientId,int newCash)
        {
            /// <summary>
            /// This Function overwirete existing account Cash
            /// </summary>
  
            var account = _dbContext.Accounts.FirstOrDefault(c => c.Id == clientId);
            if (account == null)
            {
                _logger.ErrorFormat("Unable to find clientId:{0}", clientId);
                return false;
            }
            else
            {
                account.Cash = newCash;
                _dbContext.Accounts.Update(account);
                return _dbContext.SaveChanges() > 0;
            }
        }

        public bool Buy(int clientId,string sedol,float buy)
        {
            /// <summary>
            /// This Function Buy,modify account and account.position in following 
            /// - existing Account.Cash
            /// + Executed
            /// </summary>
            /// 
            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);
            if (position == null | account == null)
            {
                return false;
            }
            else
            {   
                if (account.Cash >= buy)
                {
                    account.Cash = account.Cash - buy;
                    position.Executed = position.Executed + buy;
                    _dbContext.Positions.Update(position);
                    _dbContext.Accounts.Update(account);
                    return _dbContext.SaveChanges() > 0;
                }
                else
                {
                    _logger.WarnFormat("Not enough cash to buy: - clientId:{0},sedol:{1}, buy:{2}", clientId, sedol, buy);
                    return false;
                }
            }
        }
        public bool Sell(int clientId, string sedol, float sell)
        {
            /// <summary>
            /// This Function Sell,modify account and account.position in following 
            /// + existing Account.Cash
            /// - Position.SOD
            /// + Executed
            /// </summary>
            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);
            if (position == null | account == null)
            {
                return false;
            }
            else
            {
                if (position.Sod >= sell)
                {
                    account.Cash = account.Cash + sell;
                    position.Sod = position.Sod - sell;
                    position.Executed = position.Executed - sell;
                    _dbContext.Positions.Update(position);
                    _dbContext.Accounts.Update(account);
                    return _dbContext.SaveChanges() > 0;
                }
                else
                {
                    _logger.WarnFormat("Not enough postion to sell: - clientId:{0},sedol:{1}, sell:{2}", clientId, sedol, sell);
                    return false;
                }
            }
        }

        public bool ReleaseReserveFromSod(int clientId,string sedol, float release)
        {
            /// <summary>
            /// This Function release reserve,account.position in following 
            /// - existing Position.Reserve
            /// + existing Position.SOD
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);
            if (position == null | account == null)
            {
                _logger.ErrorFormat("acount & postion not exist: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol, release);
                return false;
            }
            else
            {
                if (position.Reserved >= release)
                {
                    position.Reserved = position.Reserved - release;
                    position.Sod = position.Sod + release;
                    _dbContext.Positions.Update(position);
                    return _dbContext.SaveChanges() > 0;
                }
                else
                {
                    position.Reserved = 0;
                    position.Sod = position.Sod + position.Reserved;
                    _dbContext.Positions.Update(position);
                    _logger.InfoFormat("release all reserve Sod: - clientId:{0},sedol:{1}, release:{2}",clientId, sedol, release);
                    return _dbContext.SaveChanges() > 0;
                }
            }
        }
        public bool MoveReserveToExecuted(int clientId, string sedol, float reserveToExecuted)
        {
            /// <summary>
            /// This Function move reserve,to executed 
            /// - existing Position.Reserve
            /// + existing Position.Executed
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);
            if (position == null | account == null)
            {
                _logger.ErrorFormat("acount & postion not exist: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol, reserveToExecuted);
                return false;
            }
            else
            {
                if (position.Reserved >= reserveToExecuted)
                {
                    position.Reserved = position.Reserved - reserveToExecuted;
                    position.Executed = position.Executed + reserveToExecuted;
                    _dbContext.Positions.Update(position);
                    return _dbContext.SaveChanges() > 0;
                }
                else
                {
                    position.Reserved = 0;
                    position.Executed = position.Executed + position.Reserved;
                    _dbContext.Positions.Update(position);
                    _logger.InfoFormat("release all reserve to Executed: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol, reserveToExecuted);
                    return _dbContext.SaveChanges() > 0;
                }
            }
        }

        public bool MakeReserve(int clientId, string sedol, float reserve)
        {
            /// <summary>
            /// This Function release reserve,account.position in following 
            /// - existing Position.Reserve
            /// + existing Position.SOD
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);
            if (position == null | account == null)
            {
                _logger.ErrorFormat("acount & postion not exist: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol, reserve);
                return false;
            }
            else
            {
                if (position.Sod >= reserve)
                {
                    position.Reserved = position.Reserved + reserve;
                    position.Sod = position.Sod - reserve;
                    _dbContext.Positions.Update(position);
                    return _dbContext.SaveChanges() > 0;
                }
                else
                {
                    position.Reserved = position.Reserved + position.Sod;
                    position.Sod = 0;
                    _dbContext.Positions.Update(position);
                    _logger.WarnFormat("move all sod to reserve: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol, reserve);
                    return _dbContext.SaveChanges() > 0;
                }
            }
        }
        public IDictionary<string, float>? CheckAcountPositionStatus(int clientId, string sedol)
        {
            /// <summary>
            /// This Function release reserve,account.position in following 
            /// - existing Position.Reserve
            /// + existing Position.SOD
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);
            var position = _dbContext.Positions.FirstOrDefault(p => p.AccountId == clientId && p.Symobl.Sedol == sedol);

            if (position != null & account != null)
            {
                IDictionary<string, float> result = new Dictionary<string, float>();
                result.Add("Cash", account.Cash);
                result.Add("SOD", position.Sod);
                result.Add("Reserved", position.Reserved);
                result.Add("Executed", position.Executed);
               return result;
               
            }
            else
            {
                _logger.ErrorFormat("acount & postion not exist: - clientId:{0},sedol:{1}, release:{2}", clientId, sedol);
                return null;
            }
        }

        public IDictionary<string, float>? CheckAccountStatus(int clientId)
        {
            /// <summary>
            /// This Function release reserve,account.position in following 
            /// - existing Position.Reserve
            /// + existing Position.SOD
            /// </summary>

            var account = _dbContext.Accounts.FirstOrDefault(p => p.Id == clientId);

            if (account != null)
            {
                IDictionary<string, float> result = new Dictionary<string, float>();
                result.Add("Cash", account.Cash);
                return result;

            }
            else
            {
                _logger.ErrorFormat("acount not exist: - clientId:{0}", clientId);
                return null;
            }
        }

        public List<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public List<Position> GetAllPositions()
        {
            return _dbContext.Positions.ToList();

        }
        public bool Delete(int clientId)
        {
            var account = _dbContext.Accounts.FirstOrDefault(s => s.Id == clientId);
            if (account == null)
            {
                return false;
            }
            _dbContext.Accounts.Remove(account);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
