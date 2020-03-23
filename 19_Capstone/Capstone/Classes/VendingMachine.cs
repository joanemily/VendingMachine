using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        #region Properties
        //Dictionary to store vending machine items slot number is the key with an Item object stored as the value
        public Dictionary<string, Item> items = new Dictionary<string, Item>();

        //Dictionary to store messages for each type of item to be displayed when an item is purchased. Key is the item type and value is the message
        public Dictionary<string, string> saleMessage = new Dictionary<string, string>()
        {
            { "Chip", "Crunch Crunch, Yum!" },
            { "Candy", "Munch Munch, Yum!" },
            { "Drink", "Glug Glug, Yum!" },
            { "Gum", "Chew Chew, Yum!" },
        };

        //Property to store the current balance the user has fed into the vending machine
        public decimal CurrentBalance { get; set; }

        //Property to store total sales, resets to 0 each time the program is started.
        public decimal GetTotalSales { get; set; }
        #endregion

        #region Constructors
        //Default Constructor resets total sales to 0 and calls stock method to restock vending machine
        public VendingMachine()
        {
            GetTotalSales = 0;
            CurrentBalance = 0;
            Stock();
        }
        #endregion

        #region Methods
        //Method to stock the vending machine should only be called once by the default constructor when a new vending machine is created
        private void Stock()
        {
            const string path = "C:\\Users\\Student\\git\\c-module-1-capstone-team-5\\19_Capstone\\vendingmachine.csv";
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    //Parse each line of the vending machine file and split it into an array of strings
                    string[] itemLine = sr.ReadLine().Split("|", StringSplitOptions.RemoveEmptyEntries);

                    //Create a new item and add it to the item dictionary by accessing the appropriate data item by index in itemLine strings
                    items.Add(itemLine[0], new Item(itemLine[1], decimal.Parse(itemLine[2]), 5, itemLine[3]));
                }
            }
        }

        //Method to purchase an item returns a decimal with the amount charged for the transaction
        public Item PurchaseItem(string slot)
        {
            //Check if the user entered an invalid slot number return 0 since no transaction takes place
            if (!items.ContainsKey(slot))
            {
                throw new PurchaseItemExceptionInvalidSlot();
            }

            //Check if the item selected is sold out, if it is return 0 as no transaction takes place
            if (items[slot].Quantity == 0)
            {
                throw new PurchaseItemExceptionItemSoldOut();
            }

            //We know the item exists in the dictionary now so set it to current Item
            Item currentItem = items[slot];

            //Check if the user has enough funds to complete the transaction if not return 0 and display a message otherwise process the transaction and return the purchase price to the caller
            if (currentItem.Price > CurrentBalance)    //Insufficient funds for purchase
            {
                throw new PurchaseItemExceptionInsufficientFunds();
            }
            else                                //Successful purchase
            {
                //Log the purchase to the audit log
                AuditLog(currentItem.Type, CurrentBalance, CurrentBalance - currentItem.Price);

                //Randomly get the item stuck in the machine.
                Random isStuck = new Random();
                bool randomBool = isStuck.Next(0, 2) > 0;
                if (randomBool)
                {
                    throw new ItemStuckException();
                    
                }
                else
                {

                    //Complete the sale by adding the purchase to Total Sales subtracting from CurrentBalance and decrementing the quantity of the item in stock
                    GetTotalSales += currentItem.Price;
                    currentItem.Quantity--;
                    CurrentBalance -= currentItem.Price;

                    //Return a formatted string containing information about the purchase
                    return currentItem;
                }
            }
        }

        //Method to feed money to the vending machine and update current balance
        public void FeedMoney(int dollar)
        {
            //Once we have a valid user input log this ammount to the audit log
            AuditLog("FEED MONEY: ", CurrentBalance, CurrentBalance + dollar);

            //Update the current balance to reflect the money fed to the machine and return to the caller
            CurrentBalance += dollar;
            return;
        }

        //Method to log new transaction to the audit log
        public void AuditLog(string transactionType, decimal previousBalance, decimal newBalance)
        {
            //Path to the audit log file
            const string PATH = "C:\\Users\\Student\\git\\c-module-1-capstone-team-5\\19_Capstone\\Log.txt";

            //Use the stream writer class to add a new entry to the audit log, the append: true option ensures it is added to the end of the existing file and old data is not overwritten
            using (StreamWriter sw = new StreamWriter(PATH, append: true))
            {
                sw.WriteLine($"{DateTime.Now} {transactionType} {previousBalance:c} {newBalance:c}");
            }
        }

        //Method to print the sales report for the current session
        public void PrintSalesReport()
        {
            string timeStamp = string.Format(("{0:yyyy-MM-dd_hh-mm-ss}"), DateTime.Now);
            string path = ($"C:\\Users\\Student\\git\\c-module-1-capstone-team-5\\19_Capstone\\SalesReports\\{timeStamp}.txt");

            using (StreamWriter report = new StreamWriter(path))
            {
                foreach (KeyValuePair<string, Item> item in items)
                {
                    report.WriteLine($"{item.Value.Name} | {5 - item.Value.Quantity}");
                }

                report.WriteLine($"Total Sales: ${GetTotalSales}");
            }
        }

        public Change MakeChange()
        {
            // Constant Values for the different coins
            const decimal Quarter = .25M;
            const decimal Dime = .10M;
            const decimal Nickel = .05M;

            // Create and initialize counters for each type of coin
            int numberOfQuarters = 0;
            int numberOfDimes = 0;
            int numberOfNickels = 0;

            // Store the starting balance so we can log the event
            decimal startingBalance = CurrentBalance;

            // Calculate the amount of each type of coin to return
            while (CurrentBalance >= .25M)
            {
                numberOfQuarters = (int)Math.Truncate(CurrentBalance / Quarter);
                CurrentBalance = CurrentBalance % .25M;
            }
            while (CurrentBalance >= .10M)
            {
                numberOfDimes = (int)Math.Truncate(CurrentBalance / Dime);
                CurrentBalance = CurrentBalance % .10M;
            }
            while (CurrentBalance >= .05M)
            {
                numberOfNickels = (int)Math.Truncate(CurrentBalance / Nickel);
                CurrentBalance = CurrentBalance % .05M;
            }

            // Log the event Give Change to the audit log
            AuditLog("GIVE CHANGE", startingBalance, CurrentBalance);

            //Return a new change object for menu to display
            return new Change(startingBalance, numberOfQuarters, numberOfDimes, numberOfNickels);
        }
        #endregion
    }
}
