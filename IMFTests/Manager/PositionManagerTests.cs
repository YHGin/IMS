using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMF.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMF.Models;
using System.ComponentModel;

namespace IMF.Manager.Tests
{
    [TestClass()]
    public class PositionManagerTests
    {
        [TestMethod()]
        public void BuySellTest()
        {
            PositionManager positionManager = new PositionManager();
            positionManager.CreateNewAccount(12345, 10000);
            positionManager.UpdateAccountCash(12345, 10000);
            Symbol symbol = positionManager.SymbolMaker("TestBuySell", "TestBuySell", "TestBuySell", 12345);
            Position position = positionManager.PositionMaker(1000, 1000, 1000, 12345, symbol);
            positionManager.AddPosition(position);
            positionManager.Buy(12345, "TestBuySell", 1000);
            positionManager.Sell(12345, "TestBuySell", 300);
            IDictionary<string, float> status = positionManager.CheckAcountPositionStatus(12345, "TestBuySell");
            bool result = (status["Cash"] == 9300) & (status["SOD"] == 700) && (status["Reserved"] == 1000) && (status["Executed"] == 1700);
            Assert.IsTrue(result);
        }


        [TestMethod()]
        public void UpdateSODPositionTest()
        {
            PositionManager positionManager = new PositionManager();
            positionManager.CreateNewAccount(22345, 10000);
            Symbol symbol = positionManager.SymbolMaker("TestUpdateSODPosition", "TestUpdateSODPosition", "TestUpdateSODPosition", 100001);
            Position position = positionManager.PositionMaker(13000, 1000, 500, 22345, symbol);
            positionManager.AddPosition(position);
            positionManager.UpdateSODPosition(29000, 22345, "TestUpdateSODPosition");
            IDictionary<string, float> status = positionManager.CheckAcountPositionStatus(22345, "TestUpdateSODPosition");
            bool result = ((status["SOD"] == 29000));
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UpdateAccountCashTest()
        {
            PositionManager positionManager = new PositionManager();
            positionManager.CreateNewAccount(99800, 1000);
            positionManager.UpdateAccountCash(99800, 50);
            IDictionary<string, float> status = positionManager.CheckAccountStatus(99800);
            bool result = (status["Cash"] == 50);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void ReleaseReserveFromSodTest()
        {
            PositionManager positionManager = new PositionManager();
            positionManager.CreateNewAccount(32345, 10000);
            positionManager.UpdateAccountCash(32345, 10000);
            Symbol symbol = positionManager.SymbolMaker("TestReleaseReserve", "TestReleaseReserve", "TestReleaseReserve", 32345);
            Position position = positionManager.PositionMaker(1000, 1000, 1000, 32345, symbol);
            positionManager.AddPosition(position);
            positionManager.ReleaseReserveFromSod(32345, "TestReleaseReserve", 500);
            IDictionary<string, float> status = positionManager.CheckAcountPositionStatus(32345, "TestReleaseReserve");
            bool result = (status["Reserved"] == 500) && (status["SOD"]==1500);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MoveReserveToExecutedTest()
        {
            string dummySedol = "TestMoveReserveToExecuted";
            int dummyAccount = 42345;
            PositionManager positionManager = new PositionManager();
            positionManager.CreateNewAccount(dummyAccount, 10000);
            positionManager.UpdateAccountCash(dummyAccount, 10000);
            Symbol symbol = positionManager.SymbolMaker(dummySedol,dummySedol,dummySedol, dummyAccount);
            Position position = positionManager.PositionMaker(1000, 1000, 1000, dummyAccount, symbol);
            positionManager.AddPosition(position);
            positionManager.MoveReserveToExecuted(dummyAccount, dummySedol, 600);
            IDictionary<string, float> status = positionManager.CheckAcountPositionStatus(dummyAccount, dummySedol);
            bool result = (status["Reserved"] == 400) && (status["Executed"] == 1600);
            Assert.IsTrue(result);
        }

    }
}