using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    static class DisplayItems
    {
        //Method to display all items in the vending machine in a grid with ASCII art needs a Vending Machine object passed as an argument
        public static void Display(VendingMachine vm)
        {
            //Grab a refererence to the Dictionary stored in Vending Machine
            Dictionary<string, Item> items = vm.items;

            Console.WriteLine("________________________________________________________________________________________");
            Console.WriteLine("|                                                                                      |");
            Console.WriteLine("|                                                                                      |");
            Console.WriteLine("|  __________________________________________________________________________________  |");
            Console.WriteLine("|  |                                                                                |  |");
            for (int i = 0; i < 4; i++)
            {
                string letter = "";
                switch (i)
                {
                    case 0:
                        letter = "A";
                        break;
                    case 1:
                        letter = "B";
                        break;
                    case 2:
                        letter = "C";
                        break;
                    case 3:
                        letter = "D";
                        break;
                }
                Console.Write("|  |");
                for (int j = 1; j <= 4; j++)
                {
                    string keyLookup = letter + j;
                    Console.Write($"{keyLookup,-20}");
                }
                Console.Write("|  |\r\n");
                Console.Write("|  |");
                for (int j = 1; j <= 4; j++)
                {
                    string keyLookup = letter + j;
                    string nameString = items[keyLookup].Name;
                    if (items[keyLookup].Quantity < 1)
                    {
                        nameString = "**SOLD OUT**";
                    }
                    Console.Write($"{nameString,-20}");
                }
                Console.Write("|  |\r\n");
                Console.Write("|  |");
                for (int j = 1; j <= 4; j++)
                {
                    string keyLookup = letter + j;
                    Console.Write($"{items[keyLookup].Price,-20:c}");
                }
                Console.Write("|  |\r\n");
                Console.WriteLine("|  |                                                                                |  |");
                Console.WriteLine("|  |                                                                                |  |");
            }
            Console.WriteLine("|  |________________________________________________________________________________|  |");
            Console.WriteLine("|                                                                                      |");
            Console.WriteLine("|           ________________________________________________________________           |");
            Console.WriteLine("|           |                                                              |           |");
            Console.WriteLine("|           |                                                              |           |");
            Console.WriteLine("|           |______________________________________________________________|           |");
            Console.WriteLine("|                                                                                      |");
            Console.WriteLine("|______________________________________________________________________________________|");
            Console.WriteLine("    |   |                                                                     |   |     ");
            Console.WriteLine("    |___|                                                                     |___|     ");


        }
    }
}
