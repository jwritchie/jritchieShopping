using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieShopping.Models.CodeFirst {

    public class CartItem {

        public int Id { get; set; }                 // Primary key
        public int ItemId { get; set; }             // Foreign key
        public string CustomerId { get; set; }      // Foreign key      guid = global unique identifier (string)
        public int Count { get; set; }  
        public DateTime Created { get; set; }


        // Virtual properties.
        public virtual Item Item { get; set; }                  // Truncate ItemId ... Entity framework based.  One item per cart item.
        public virtual ApplicationUser Customer { get; set; }  // Whose cart is this? Who is the owner?     Points at ApplicationUser object.



        public decimal unitTotal {

            get {

                if (Item != null)
                {
                    return Count * Item.Price;
                }

                return 0;
            }
        }
    }
}