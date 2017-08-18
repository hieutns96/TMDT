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
    public class HoaDonController : Controller
    {
        private wthEntities db = new wthEntities();

        // GET: /HoaDon/
        public ActionResult Index()
        {
            return View(db.HOA_DON.ToList());
        }

        // GET: /HoaDon/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hoa_don = db.HOA_DON.Find(id);
            if (hoa_don == null)
            {
                return HttpNotFound();
            }
            return View(hoa_don);
        }

        // GET: /HoaDon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HoaDon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MA_HD,USERNAME,IDTT,DIA_CHI,HO_TEN,DIEN_THOAI,NGAYDATHANG,GHI_CHU,TONG_TIEN")] HOA_DON hoa_don)
        {
            if (ModelState.IsValid)
            {
                db.HOA_DON.Add(hoa_don);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoa_don);
        }

        // GET: /HoaDon/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hoa_don = db.HOA_DON.Find(id);
            if (hoa_don == null)
            {
                return HttpNotFound();
            }
            return View(hoa_don);
        }

        // POST: /HoaDon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MA_HD,USERNAME,IDTT,DIA_CHI,HO_TEN,DIEN_THOAI,NGAYDATHANG,GHI_CHU,TONG_TIEN")] HOA_DON hoa_don)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoa_don).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoa_don);
        }

        // GET: /HoaDon/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hoa_don = db.HOA_DON.Find(id);
            if (hoa_don == null)
            {
                return HttpNotFound();
            }
            return View(hoa_don);
        }

        // POST: /HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            HOA_DON hoa_don = db.HOA_DON.Find(id);
            db.HOA_DON.Remove(hoa_don);
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
