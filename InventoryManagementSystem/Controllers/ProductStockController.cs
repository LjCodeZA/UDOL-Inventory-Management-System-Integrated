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

namespace InventoryManagementSystem.Controllers
{
    public class ProductStockController : Controller
    {
        private IMSContext db = new IMSContext();

        // GET: ProductStock
        public ActionResult Index()
        {
            var productStock = db.ProductStock.Include(p => p.ProductVendor);
            return View(productStock.ToList());
        }

        // GET: ProductStock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = db.ProductStock.Find(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            return View(productStock);
        }

        // GET: ProductStock/Create
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

            //ViewBag.ProductVendorId = new SelectList(db.ProductVendor, "ProductVendorId", "ProductVendorId");
            return View();
        }

        // POST: ProductStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductStockId,ProductVendorId,Quantity")] ProductStock productStock)
        {
            if (ModelState.IsValid)
            {
                productStock.StockTakeDate = DateTime.Now;
                db.ProductStock.Add(productStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductVendorId = new SelectList(db.ProductVendor, "ProductVendorId", "ProductVendorId", productStock.ProductVendorId);
            return View(productStock);
        }

        // GET: ProductStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = db.ProductStock.Find(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductVendorId = new SelectList(db.ProductVendor, "ProductVendorId", "ProductVendorId", productStock.ProductVendorId);
            return View(productStock);
        }

        // POST: ProductStock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductStockId,ProductVendorId,Quantity,StockTakeDate")] ProductStock productStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductVendorId = new SelectList(db.ProductVendor, "ProductVendorId", "ProductVendorId", productStock.ProductVendorId);
            return View(productStock);
        }

        // GET: ProductStock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock productStock = db.ProductStock.Find(id);
            if (productStock == null)
            {
                return HttpNotFound();
            }
            return View(productStock);
        }

        // POST: ProductStock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductStock productStock = db.ProductStock.Find(id);
            db.ProductStock.Remove(productStock);
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
