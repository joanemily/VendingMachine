using Capstone.Classes;
using System;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new vending machine object which will also stock the vending machine
            VendingMachine vm = new VendingMachine();

            //Create a new menu object passing it the vending machine object we created
            Menu mn = new Menu(vm);

            //Call the display method for the menu, all further program control handled by the menu classes
            mn.Display();
        }
    }
}
