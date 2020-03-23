using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineUnitTests
    {
        [DataTestMethod]
        [DataRow("0", 0, 0, 0)]
        [DataRow("10", 40, 0, 0)]
        [DataRow("3.25", 13, 0, 0)]
        [DataRow("3.40", 13, 1, 1)]
        [DataRow("6.20", 24, 2, 0)]
        [DataRow("9.15", 36, 1, 1)]
        [DataRow("0.45", 1, 2, 0)]
        [DataRow("-3.40", 0, 0, 0)]
        public void MakeChangeTests(string startingValue, int expectedQuarters, int expectedDimes, int expectedNickels)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            vm.CurrentBalance = decimal.Parse(startingValue);
            decimal expectedChange = decimal.Parse(startingValue);

            //Act
            Change actual = vm.MakeChange();

            //Assert
            Assert.AreEqual(expectedChange, actual.TotalChangeAmount);
            Assert.AreEqual(expectedQuarters, actual.Quarters);
            Assert.AreEqual(expectedDimes, actual.Dimes);
            Assert.AreEqual(expectedNickels, actual.Nickels);
        }

        [DataTestMethod]
        [DataRow(2, "2.00")]
        [DataRow(5, "5.00")]
        [DataRow(0, "0.00")]
        [DataRow(12, "12.00")]
        public void FeedMoneyTests(int dollar, string expectedVal)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            decimal expectedValue = decimal.Parse(expectedVal);

            //Act
            vm.FeedMoney(dollar);
            decimal actualValue = vm.CurrentBalance;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow("A1", "Potato Crisps")]
        [DataRow("A4", "Cloud Popcorn")]
        [DataRow("B2", "Cowtales")]
        [DataRow("B3", "Wonka Bar")]
        [DataRow("C2", "Dr. Salt")]
        [DataRow("D3", "Chiclets")]
        public void PurchaseValidItemTest(string slotToPurchase, string expectedValue)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            vm.CurrentBalance = 10M;
            
            //Act
            string actualValue = vm.PurchaseItem(slotToPurchase).Name;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow("A11")]
        [DataRow("F4")]
        [DataRow("B5")]
        [DataRow("E7")]
        [DataRow("F99")]
        [DataRow("C9")]
        public void PurchaseItemThrowExceptionInvalidSlotTest(string slotToPurchase)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            vm.CurrentBalance = 10M;

            //Act and Assert
            Assert.ThrowsException<PurchaseItemExceptionInvalidSlot>(
            () => vm.PurchaseItem(slotToPurchase));
        }

        [DataTestMethod]
        [DataRow("A1")]
        [DataRow("B2")]
        [DataRow("A4")]
        [DataRow("B3")]
        [DataRow("C2")]
        [DataRow("D4")]
        public void PurchaseItemThrowExceptionSoldOutTest(string slotToPurchase)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            vm.items[slotToPurchase].Quantity = 0;

            //Act and Assert
            Assert.ThrowsException<PurchaseItemExceptionItemSoldOut>(
            () => vm.PurchaseItem(slotToPurchase));
        }

        [DataTestMethod]
        [DataRow("A3")]
        [DataRow("B1")]
        [DataRow("A2")]
        [DataRow("B4")]
        [DataRow("C3")]
        [DataRow("D1")]
        public void PurchaseItemThrowExceptionInsufficientFundsTest(string slotToPurchase)
        {
            //Arrange
            VendingMachine vm = new VendingMachine();
            vm.CurrentBalance = 0;

            //Act and Assert
            Assert.ThrowsException<PurchaseItemExceptionInsufficientFunds>(
            () => vm.PurchaseItem(slotToPurchase));
        }
    }
}

