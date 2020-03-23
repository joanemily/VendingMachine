using Capstone.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class Menu
    {
        #region Properties
        private VendingMachine vm { get; }
        #endregion

        #region Constructors
        public Menu(VendingMachine vm)
        {
            this.vm = vm;
        }
        #endregion

        #region Methods
        public void Display ()
        {
            Console.Clear();
            //Display menu options
            Console.WriteLine(@"
    _______    ______     ___    __                          __
   /__  __//  / ____//    | ||  / //________________________/ //__(_))___________________ 
     / //    / __//       | || / // /  _\\  / ___ \\  /  __  //  / //  / ___ \\  / ___ `//
    / //    / //___       | ||/ // /  __// / // / // / //_/ //  / //  / // / // / //_/ // 
   /_//    /_____//       |____//  \___// /_// /_//  \___,_//  /_//  /_// /_//  \__,  //  
                                                                               /_____//                            
                    *************************************************
                    **  Please choose from the following options:  **
                    **      1. Display Vending Machine Items       **
                    **      2. Purchase Menu                       **
                    **      3. Exit                                **
                    **                                             **
                    *************************************************");

            //Get user input
            int selection;
            Console.WriteLine();
            Console.Write("Please enter your selection: ");
            bool isValid = int.TryParse(Console.ReadLine(), out selection);

            //Validate the user input
            while (!isValid || selection < 1 || selection > 4)
            {
                Console.WriteLine("You did not enter a valid choice please enter the number corresponding to your selection");
                isValid = int.TryParse(Console.ReadLine(), out selection);
            }

            // Choose which methods to call based on user selection
            if (selection == 1)
            {
                // Clear the console and display Menu Items
                Console.Clear();
                DisplayItems.Display(vm);
                Console.WriteLine();
                Console.WriteLine("Press Enter to return to main menu");
                Console.ReadLine();
                Display();
            }
            else if (selection == 2)
            {
                // Display Purchase Menu by creating a new purchase menu object and passing the vending machine reference to its contructor
                PurchaseMenu pm = new PurchaseMenu(vm);
                pm.Display();
            }
            else if (selection == 3)
            {
                // Exit Program
                return;
            } 
            else if(selection == 4)
            {
                // Generate a sales report and print it to file.
                vm.PrintSalesReport();

                //Inform the user that a report was generated and return to main menu
                Console.WriteLine();
                Console.WriteLine("Sales report generated, check the SalesReport folder to view the report.");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                Display();
            }
        }

        //public void DisplayItems()
        //{
        //    Dictionary<string, Item> items = vm.items;

        //    foreach (KeyValuePair<string, Item> kvp in items)
        //    {
        //        //If the item is sold out change display value to SOLD OUT otherwise display quantity remaining
        //        if (kvp.Value.Quantity == 0)
        //        {
        //            Console.WriteLine($"{kvp.Key}) {kvp.Value.Name} - {kvp.Value.Price} - SOLD OUT");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"{kvp.Key}) {kvp.Value.Name} - {kvp.Value.Price} - {kvp.Value.Quantity}");
        //        }

        //    }
        //}
        #endregion
    }
}
