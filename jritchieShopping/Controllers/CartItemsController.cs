using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jritchieShopping.Models;
using jritchieShopping.Models.CodeFirst;

using Microsoft.AspNet.Identity;

namespace jritchieShopping.Controllers
{
    public class CartItemsController : Universal
    {
        // GET: CartItems
        [Authorize]                                 // Requires user be logged in; if not, redirects the user to the log-in page.
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(user.CartItems.ToList());   // Filters CartItems to show only the current user's items. 
            //return View(db.CartItems.ToList());   // Shows all CartItems.
        }

        // GET: CartItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // GET: CartItems/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: CartItems/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ItemId,CustomerId,Count,Created")] CartItem cartItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CartItems.Add(cartItem);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cartItem);
        //}

        // POST: CartItems/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int? id, int? quantity)
        {
            //if (user == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            int increment = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            if (quantity != null)
            {
                increment = quantity.Value;
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            if (db.CartItems.Where(i => i.CustomerId == user.Id).Any(i => i.ItemId == id.Value))
            {
                var existingCartItem = db.CartItems.Where(i => i.CustomerId == user.Id).FirstOrDefault(i => i.ItemId == id.Value);
                existingCartItem.Count += increment;
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            else
            {
                CartItem cartItem = new CartItem();
                cartItem.Count = increment;
                cartItem.ItemId = id.Value;
                cartItem.Created = DateTime.Now;
                cartItem.CustomerId = user.Id;

                db.CartItems.Add(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");               // CartItem Index view.
        }

        // GET: CartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,CustomerId,Count,Created")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
