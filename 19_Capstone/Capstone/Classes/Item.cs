using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Item
    {
        #region Properties
        //Property to store the name of the item
        public string Name { get; private set; }
        //Property to store the price of an item
        public decimal Price { get; private set; }
        //Property to store the quantity of an item in stock
        public int Quantity { get; set; }
        //Property to store the item type
        public string Type { get; private set; }
        
        #endregion

        #region Constructors
        //The only contructor for this class requires all properties to be passed
        public Item(string name, decimal price, int quantity, string type)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
        }
        #endregion
    }
}
