using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Change
    {
        #region Properties
        public decimal TotalChangeAmount { get; set; }
        public int Quarters { get; set; }
        public int Dimes { get; set; }
        public int Nickels { get; set; }
        #endregion

        #region Constructors
        //Only use this constructor to instantiate a new change object
        public Change(decimal totalChangeAmount, int quarters, int dimes, int nickels)
        {
            TotalChangeAmount = totalChangeAmount;
            Quarters = quarters;
            Dimes = dimes;
            Nickels = nickels;
        }
        #endregion
    }
}
