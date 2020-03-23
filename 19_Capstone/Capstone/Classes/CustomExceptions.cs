using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    //Create 3 new exceptions with no properties for Purchase Item method testing
    public class PurchaseItemExceptionInvalidSlot : Exception
    {
    }
    public class PurchaseItemExceptionItemSoldOut : Exception
    {
    }
    public class PurchaseItemExceptionInsufficientFunds : Exception
    {
    }

    public class ItemStuckException : Exception
    {

    }
}

