using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hieu_TMDT.Models;
using Terradue.ServiceModel.Syndication;
using RSSFeed.Website.Models;


namespace Hieu_TMDT.Controllers
{
    public class HomeController : Controller
    {
        private wthEntities db = new wthEntities();
        NhaSachRepository ns = new NhaSachRepository();
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Rss()
        {
            List<SyndicationItem> items1 = new List<SyndicationItem>();
            List<Models.MyBook> items2 = ns.GetNewBooks();
            for (int i = 0; i < 9; i++)
            {
                string imgURL = "<img src=\"";
                imgURL += "http://localhost/Image/Sach/" + items2[i].AnhBia + "\"style=\"width:180px; height:230px; margin-top:9px;\"/><br>";
                SyndicationItem item = new SyndicationItem()
                {
                    Id = items2[i].MaSach.ToString(),
                    Title = SyndicationContent.CreatePlaintextContent(String.Format("{0}", items2[i].TenSach)),

                    Content = SyndicationContent.CreateHtmlContent(String.Format("{0} Giá bán: {1} <br>", imgURL, items2[i].GiaBan.ToString())),
                };
                string mylink = "http://localhost/Product/Product/" + ns.Encode("id=" + items2[i].MaSach.ToString());
                item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(mylink)));//link to item
                items1.Add(item);
            }
            var feed = new SyndicationFeed("Usagi's Store", "Cửa hàng sách Usagi", new Uri("http://localhost"), items1)
            {
                Copyright = new TextSyndicationContent("© 2015 - Usagi's Store"),
                Language = "vi-VN"
            };
            Response.ContentType = "text/xml";
            return new FeedResult(new Rss20FeedFormatter(feed));
        }

        [HttpGet]
        public ActionResult GetBooks()
        {
            List<MyBook> items = ns.GetAllBooks();
            return new JsonResult() { Data = new { Data = items }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult GetBestSeller()
        {
            List<MyBook> items = ns.BestSeller();
            return new JsonResult() { Data = new { Data = items }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult GetNewBooks()
        {
            List<MyBook> items = ns.GetNewBooks();
            return new JsonResult() { Data = new { Data = items }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}