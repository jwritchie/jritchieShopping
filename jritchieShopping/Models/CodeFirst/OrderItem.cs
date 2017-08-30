using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieShopping.Models.CodeFirst {

    public class OrderItem {

        public int Id { get; set; }             // Primary key      
        public int OrderId { get; set; }        // Foreign key
        public int ItemId { get; set; }         // Foreign key
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Virtual properties.
        public virtual Item Item { get; set; }      // Allows access to all properties of another object, does not get added to the database.
        public virtual Order Order { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}