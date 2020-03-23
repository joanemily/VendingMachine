using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class PurchaseMenu
    {
        #region Properties
        //Private property to store a read-only reference to a vending machine object
        private VendingMachine vm { get; }
        #endregion

        #region Constructors
        //Constructor that requires a reference to a vending machine object this is the only constructor that should be used
        public PurchaseMenu(VendingMachine vm)
        {
            this.vm = vm;
        }
        #endregion

        #region Methods
        //Method to display the purchase menu
        public void Display()
        {
            //Clear the console whenever the display method is called
            Console.Clear();

            //Display menu options to the user
            Console.WriteLine($@"
    _______    ______     ___    __                          __
   /__  __//  / ____//    | ||  / //________________________/ //__(_))___________________ 
     / //    / __//       | || / // /  _\\  / ___ \\  /  __  //  / //  / ___ \\  / ___ `//
    / //    / //___       | ||/ // /  __// / // / // / //_/ //  / //  / // / // / //_/ // 
   /_//    /_____//       |____//  \___// /_// /_//  \___,_//  /_//  /_// /_//  \__,  //  
                                                                               /_____// 
                    *************************************************
                    **  Please choose from the following options:  **
                    **           1. Feed Money                     **
                    **           2. Select Product                 **
                    **           3. Finish Transaction             **
                    **                                             ** 
                    **       Current Money Provided: {vm.CurrentBalance, 7:c}       **
                    *************************************************
");

            //Call method to get input from the user
            GetUserMenuInput();
        }

        //Method to recieve and validate user input and take the appropriate action based on user menu selection
        private void GetUserMenuInput()
        {
            //Prompt the user to make a selection and recieve input from the user
            int selection;
            Console.WriteLine();
            Console.Write("Enter your selection: ");
            bool isValid = int.TryParse(Console.ReadLine(), out selection);

            //Validate the user input
            while (!isValid || selection < 1 || selection > 3)
            {
                Console.WriteLine("You did not enter a valid choice please enter the number corresponding to your selection");
                isValid = int.TryParse(Console.ReadLine(), out selection);
            }

            //Check which option the user selected and call the appropriate methods
            switch (selection)
            {
                case 1:                     //Call the feed money method and then recall display after the method returns
                    vm.FeedMoney(GetMoneyToFeed());
                    Display();
                    break;
                case 2:
                    PurchaseItemSubMenu();  //Display the Purchase Item submenu and then recall display when it returns
                    Display();
                    break;
                case 3:                     //Call the make change method and then create a new menu object and call display when make change returns
                    DisplayChange();
                    Menu mn = new Menu(vm);
                    mn.Display();
                    break;
            }
        }

        //Method to Display Change when the user exits the purchase menu
        private void DisplayChange()
        {
            Console.WriteLine();

            //Create a new change object and call Make Change
            Change change = vm.MakeChange();

            //Check if the change object holds no change
            if (change.TotalChangeAmount == 0.00M)
            {
                Console.WriteLine("You had no remaining balance to make change with.");
            }
            else                //Change object has change so print that change
            {
                Console.WriteLine(
$@"You are receiving {change.TotalChangeAmount:c} in change

Dispensing {change.Quarters} Quarters, {change.Dimes} Dimes, and {change.Nickels} nickels");
            }

            //After returning from Make Change wait for the user to press enter before going back to main purchase menu
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue..");
            Console.ReadLine();
        }

        //Method to get input from the user to feed to the machine
        private int GetMoneyToFeed()
        {
            //Prompt the user to enter an amount of dollars to feed in whole dollar amounts
            Console.WriteLine("How many dollars do you want to feed?: ");
            int dollar;
            bool isValid = int.TryParse(Console.ReadLine(), out dollar);

            //Data validation
            while (!isValid)
            {
                Console.WriteLine("You did not enter a valid amount please enter a whole dollar amount: ");
                isValid = int.TryParse(Console.ReadLine(), out dollar);
            }

            return dollar;
        }

        //Method to display a submenu that allows the user to purchase an item by slot ID
        private void PurchaseItemSubMenu()
        {
            //Clear the console before displaying the purchase submenu
            Console.Clear();
            //Display all items in the vending machine
            DisplayItems.Display(vm);

            //Ask the user to enter a slot number corresponding to the item they want to purchase
            Console.WriteLine();
            Console.Write("Please enter an item to purchase by slot (A1, B3, etc): ");
            string input = Console.ReadLine().ToUpper();            //Add a ToUpper method to user input for case insensitivity 

            //Pass user input to purchase item method which will determine if it is a valid purchase
            Item purchasedItem = null;
            try
            {
                purchasedItem = vm.PurchaseItem(input);
            }
            catch (PurchaseItemExceptionInvalidSlot)
            {
                Console.WriteLine();
                Console.WriteLine("The slot entered does not exist, returning to the purchase menu");
            }
            catch (PurchaseItemExceptionItemSoldOut)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry but the item you selected is sold out, returning to the purchase menu");
            }
            catch (PurchaseItemExceptionInsufficientFunds)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry you do not have enough funds to purchase that item, returning to the purchase menu");
            }
            catch (ItemStuckException)
            {
                Console.WriteLine();
                Console.WriteLine($"The {purchasedItem.Name} you have purchased appears to be stuck, what would you like to do?");

                Console.WriteLine(@" 1. Return to Purchase Menu.
                                     2. Kick the Machine.");
                string input1 = Console.ReadLine();
                int response = int.Parse(input1);
                if (response < 1 || response > 2)
                {
                    Console.WriteLine("You have made an incorrect selection. Please try again.");
                    return;
                }
                else 
                {
                    IsStuck(response);
                }
            }

            //If purchase is succesful display item info
            if (purchasedItem != null)
            {
                Console.WriteLine($@"
You succesfully purchased {purchasedItem.Name}!
{vm.saleMessage[purchasedItem.Type]}

{purchasedItem.Price:c} has been deducted from your balance!");
            }

            //After returning from purchase item wait for the user to press enter before going back to main purchase menu
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue..");
            Console.ReadLine();
        }

        public void IsStuck(int response)
        {
            if (response == 1)
            {
                Console.Clear();
                GetUserMenuInput();

            }
            else if (response == 2)
            {
                Console.Clear();
                Console.WriteLine($@"
   
                ******       ***       ****    ****
                **   **     ** **      ** **  ** **
                **  **     **   **     **  ****  **
                *****     *********    **   **   **
                **   **  **       **   **        **
                ******  **         **  **        **  
                ");

                Item purchasedItem = null;
                Random isUnstuck = new Random();
                bool randomBool = isUnstuck.Next(0, 2) > 1;
                if (randomBool)
                {
                    Console.WriteLine($"{purchasedItem.Name} has been released! Please Enjoy!");
                    GetUserMenuInput();
                }
                else
                {
                    Console.WriteLine($"{purchasedItem.Name} remains stuck... please make another selection.");
                    GetUserMenuInput();
                }

            }
        }
        #endregion
    }
}

