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
using System.IO;

namespace jritchieShopping.Controllers
{
    public class ItemsController : Universal
    {

        // GET: Items
        public ActionResult Index(string searchTerm)
        {
            if (searchTerm == null)
            {
                return View(db.Items.ToList());
            }

            List<Item> searchList = new List<Item>();
            foreach(Item item in db.Items)
            {
                if(item.Name.Contains(searchTerm) || item.Description.Contains(searchTerm))
                {
                    searchList.Add(item);
                }
            }

            if (searchList.Count > 0)
            { 
                return View(searchList);
            }
            else
            {
                return View(db.Items.ToList());
            }
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]    // This is an Attribute added to the 'Create' method to limit this to only users with 'Admin' access.
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreateDate,UpdatedDate,Name,Price,MediaURL,Description")] Item item, HttpPostedFileBase image)
        {
            // Validate image.
            if (image != null && image.ContentLength > 0)       // Confirm file has data
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format.");       // Validation message
                }
            }

            // Test whether all properties were received.
            if (ModelState.IsValid)     // If invalid, redirects back and displays validation error message.
            {
                var filePath = "/assets/Images/Uploads/";                   // MediaURL
                var absPath = Server.MapPath("~" + filePath);               // Physical file

                if (image == null)
                {
                    item.MediaURL = "/assets/Images/DefaultImage.jpg";          // Set to default image if no image is specified
                }
                else
                {
                    item.MediaURL = filePath + image.FileName;              // Sets path of file in database
                    image.SaveAs(Path.Combine(absPath, image.FileName));    // Saves (adds) the physical file to the application
                }

                item.CreateDate = System.DateTime.Now;

                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateDate,Name,Price,MediaURL,Description")] Item item, string mediaURL, HttpPostedFileBase image)
        {
            // Validate image.
            if (image != null && image.ContentLength > 0)       // Confirm file has data
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format.");       // Validation message
                }
            }

            // Test whether all properties were received.
            if (ModelState.IsValid)     // If invalid, redirects back and displays validation error message.
            {

                /*** If the user leaves the image field empty... ***/

                /* This version sets the image to the default image. */
                //var filePath = "/assets/Images/Uploads/";                       // MediaURL
                //var absPath = Server.MapPath("~" + filePath);                   // Physical file

                //if (image == null)
                //{
                //    item.MediaURL = "/assets/Images/DefaultImage.jpg";          // Set to default image if no image is specified
                //}
                //else
                //{
                //    item.MediaURL = filePath + image.FileName;                  // Sets path of file in database
                //    image.SaveAs(Path.Combine(absPath, image.FileName));        // Saves (adds) the physical file to the application
                //}


                /* This version sets the image to the prior (current) image. */
                db.Entry(item).State = EntityState.Modified;
                if (image != null)
                {
                    var filePath = "/assets/Images/Uploads/";                 // MediaURL
                    var absPath = Server.MapPath("~" + filePath);             // Physical file
                    item.MediaURL = filePath + image.FileName;                // Sets path of file in database
                    image.SaveAs(Path.Combine(absPath, image.FileName));      // Saves (adds) the physical file to the application
                }
                else
                {
                    item.MediaURL = mediaURL;                                 // Sets image to prior image. 
                }

                item.UpdatedDate = System.DateTime.Now;
                
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);

            // Don't delete the DefaultImage.jpg!
            if (item.MediaURL != "/assets/Images/DefaultImage.jpg")
            {
                var absPath = Server.MapPath("~" + item.MediaURL);          // Identify the Physical file.
                System.IO.File.Delete(absPath);                             // Delete the physical file.
                // File.Delete(absPath);                                    // This doesn't work, attempts to access 'File' within the Controller class.
            }
            db.Items.Remove(item);
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
