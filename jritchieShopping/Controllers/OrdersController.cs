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
    public class OrdersController : Universal
    {

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            // Set default values for Name and Email properties.
            var model = new Order
            {
                Name = ViewBag.Fullname,
                Email = User.Identity.Name
            };

            return View(model);
        }

        //// GET: Orders/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}


        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Name,Email")] Order order)
        {
            order.Total = ViewBag.CartTotal;
            order.OrderDate = DateTime.Now;

            var userId = User.Identity.GetUserId();
            order.CustomerId = userId;

            var cartItems = db.CartItems.Where(c => c.CustomerId == userId).ToList();



            if (ModelState.IsValid)
            {
                // Add Order to Orders table.
                db.Orders.Add(order);
                db.SaveChanges();

                // Add OrderItem to OrderItems table.
                foreach (var cartItem in cartItems)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.OrderId = order.Id;
                    orderItem.ItemId = cartItem.ItemId;
                    orderItem.Quantity = cartItem.Count;
                    orderItem.UnitPrice = cartItem.Item.Price;

                    db.OrderItems.Add(orderItem);
                    db.CartItems.Remove(cartItem);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", order);
            }

            return View(order);

            // public class OrderItem
            //       {

            //            public int Id { get; set; }             // Primary key      
            //            public int OrderId { get; set; }        // Foreign key
            //            public int ItemId { get; set; }         // Foreign key
            //            public int Quantity { get; set; }
            //            public decimal UnitPrice { get; set; }


            //********************************************************

            // public class CartItem
            //            {

            //                public int Id { get; set; }                 // Primary key
            //                public int ItemId { get; set; }             // Foreign key
            //                public string CustomerId { get; set; }      // Foreign key      guid = global unique identifier (string)
            //                public int Count { get; set; }
            //                public DateTime Created { get; set; }

        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Orders/Submit
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                order.Submitted = true;

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return View(order);
            }
            return View(order);
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
