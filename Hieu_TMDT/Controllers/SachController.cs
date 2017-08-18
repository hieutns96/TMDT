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
    public class SachController : Controller
    {
        private wthEntities db = new wthEntities();

        // GET: /Sach/
        public ActionResult Index()
        {
            return View(db.SACHes.ToList());
        }

        // GET: /Sach/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: /Sach/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Sach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MA_SACH,MA_DM,MA_NXB,TEN_SACH,GIA_BIA,GIA_BAN,NGAY_NHAP,SOLUOTMUA,ANHBIA,TINHTRANG,LUOT_XEM")] SACH sach)
        {
            if (ModelState.IsValid)
            {
                db.SACHes.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sach);
        }

        // GET: /Sach/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: /Sach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MA_SACH,MA_DM,MA_NXB,TEN_SACH,GIA_BIA,GIA_BAN,NGAY_NHAP,SOLUOTMUA,ANHBIA,TINHTRANG,LUOT_XEM")] SACH sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }

        // GET: /Sach/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: /Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SACH sach = db.SACHes.Find(id);
            db.SACHes.Remove(sach);
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
