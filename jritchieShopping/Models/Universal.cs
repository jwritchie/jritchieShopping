using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace jritchieShopping.Models
{
    public class Universal : Controller
    {
        // Allow communication with database.
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated) {

                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;


                // LINQ:  c => c.CustomerId  iterates through all customerIds. 'c' is a variable. This is similar to a foreach loop.
                // We want cartitems where customerId == current logged-in user's id.

                ViewBag.CartItems = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList();     // Close db connection after each access (no tracking). 
                //ViewBag.CartItems = user.CartItems.ToList(); 



                // Ryan's code for total cart items using LINQ.
                //ViewBag.TotalCartItems = user.CartItems.Sum(c => c.Count);

                if (ViewBag.CartItems.Count != 0)
                { 
                    ViewBag.TotalCartItems = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).Sum(c => c.Count);
                    ViewBag.CartTotal = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).Sum(c => c.Item.Price * c.Count);
                }
                else
                {
                    ViewBag.TotalCartItems = 0;
                    ViewBag.CartTotal = 0;
                }

                


                /***  Below code is from Ryan's project ***/
                //ViewBag.ItemTypes = db.ItemTypes.AsNoTracking().OrderBy(t => t.TypeName).ToList();
                //ViewBag.CartItems = user.CartItems.ToList();
                //decimal count = 0;
                //foreach (var cartItem in user.CartItems) {
                //    if (cartItem Item.OnSale == true) {

                //        count += cartItem.Item.SalePrice.Value * Convert.ToDecimal(cartItem.Count);
                //    } else {

                //        count += cartItem.Item.Price * Convert.ToDecimal(cartItem.Count);
                //    }
                //}
                //ViewBag.CartItemsTotalCost = count;

                //var latestItems = new List<Item>();
                //var myItems = db.Items.AsNoTracking().OrderByDescending(i => i.Created).ToList();
                //int Litem = 0;
                //foreach (var latestItem in myItems) {

                //}

            }

            base.OnActionExecuting(filterContext);
        }

    }
}