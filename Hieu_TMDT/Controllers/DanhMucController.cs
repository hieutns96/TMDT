using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hieu_TMDT;

namespace Hieu_TMDT.Controllers
{
    public class DanhMucController : Controller
    {
        private wthEntities db = new wthEntities();

        // GET: /DanhMuc/
        public ActionResult Index()
        {
            return View(db.DANH_MUC.ToList());
        }

        // GET: /DanhMuc/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC danh_muc = db.DANH_MUC.Find(id);
            if (danh_muc == null)
            {
                return HttpNotFound();
            }
            return View(danh_muc);
        }

        // GET: /DanhMuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /DanhMuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MA_DM,TEN_DM")] DANH_MUC danh_muc)
        {
            if (ModelState.IsValid)
            {
                db.DANH_MUC.Add(danh_muc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danh_muc);
        }

        // GET: /DanhMuc/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC danh_muc = db.DANH_MUC.Find(id);
            if (danh_muc == null)
            {
                return HttpNotFound();
            }
            return View(danh_muc);
        }

        // POST: /DanhMuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MA_DM,TEN_DM")] DANH_MUC danh_muc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danh_muc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danh_muc);
        }

        // GET: /DanhMuc/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANH_MUC danh_muc = db.DANH_MUC.Find(id);
            if (danh_muc == null)
            {
                return HttpNotFound();
            }
            return View(danh_muc);
        }

        // POST: /DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DANH_MUC danh_muc = db.DANH_MUC.Find(id);
            db.DANH_MUC.Remove(danh_muc);
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
