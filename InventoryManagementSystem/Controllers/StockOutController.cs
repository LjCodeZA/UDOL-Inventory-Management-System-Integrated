using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.POCO;

namespace InventoryManagementSystem.Controllers
{
    public class StockOutController : Controller
    {
        private IMSContext db = new IMSContext();

        // GET: StockOuts
        public ActionResult Index()
        {
            return View(db.StockOut.ToList());
        }

        // GET: StockOuts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOut stockOut = db.StockOut.Find(id);
            if (stockOut == null)
            {
                return HttpNotFound();
            }
            return View(stockOut);
        }

        // GET: StockOuts/Create
        public ActionResult Create()
        {
            var vendorProduct = (from productVendor in db.ProductVendor
                                 join vendor in db.Vendor
                                   on productVendor.VendorId equals vendor.VendorId
                                 join product in db.Product
                                  on productVendor.ProductId equals product.ProductId
                                 select new SelectListItem
                                 {
                                     Value = productVendor.ProductVendorId.ToString(),
                                     Text = productVendor.ProductVendorId.ToString() + " - " + vendor.Name + " - " + product.Name
                                 }).ToList();

            ViewBag.ProductVendorId = vendorProduct;
            return View();
        }

        // POST: StockOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockOutId,ProductVendorId,Quantity,Recon,CreatedDate")] StockOut stockOut)
        {
            var CommonMethods = new Common.CommonUse();
            if (ModelState.IsValid)
            {
                CommonMethods.DoStockAllocation(stockOut);
                return RedirectToAction("Index");
            }

            return View(stockOut);
        }

        // GET: StockOuts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOut stockOut = db.StockOut.Find(id);
            if (stockOut == null)
            {
                return HttpNotFound();
            }
            return View(stockOut);
        }

        // POST: StockOuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockOutId,ProductVendorId,Quantity,CreatedDate")] StockOut stockOut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockOut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockOut);
        }

        // GET: StockOuts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOut stockOut = db.StockOut.Find(id);
            if (stockOut == null)
            {
                return HttpNotFound();
            }
            return View(stockOut);
        }

        // POST: StockOuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockOut stockOut = db.StockOut.Find(id);
            db.StockOut.Remove(stockOut);
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
