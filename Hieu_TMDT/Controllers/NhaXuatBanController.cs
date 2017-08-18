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
    public class NhaXuatBanController : Controller
    {
        private wthEntities db = new wthEntities();

        // GET: /NhaXuatBan/
        public ActionResult Index()
        {
            return View(db.NHA_XUAT_BAN.ToList());
        }

        // GET: /NhaXuatBan/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_XUAT_BAN nha_xuat_ban = db.NHA_XUAT_BAN.Find(id);
            if (nha_xuat_ban == null)
            {
                return HttpNotFound();
            }
            return View(nha_xuat_ban);
        }

        // GET: /NhaXuatBan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /NhaXuatBan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MA_NXB,TEN_NXB")] NHA_XUAT_BAN nha_xuat_ban)
        {
            if (ModelState.IsValid)
            {
                db.NHA_XUAT_BAN.Add(nha_xuat_ban);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nha_xuat_ban);
        }

        // GET: /NhaXuatBan/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_XUAT_BAN nha_xuat_ban = db.NHA_XUAT_BAN.Find(id);
            if (nha_xuat_ban == null)
            {
                return HttpNotFound();
            }
            return View(nha_xuat_ban);
        }

        // POST: /NhaXuatBan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MA_NXB,TEN_NXB")] NHA_XUAT_BAN nha_xuat_ban)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nha_xuat_ban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nha_xuat_ban);
        }

        // GET: /NhaXuatBan/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHA_XUAT_BAN nha_xuat_ban = db.NHA_XUAT_BAN.Find(id);
            if (nha_xuat_ban == null)
            {
                return HttpNotFound();
            }
            return View(nha_xuat_ban);
        }

        // POST: /NhaXuatBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            NHA_XUAT_BAN nha_xuat_ban = db.NHA_XUAT_BAN.Find(id);
            db.NHA_XUAT_BAN.Remove(nha_xuat_ban);
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
