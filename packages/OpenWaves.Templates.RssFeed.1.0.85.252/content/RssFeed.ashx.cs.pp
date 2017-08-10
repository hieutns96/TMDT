using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using OpenWaves.EPiServer;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Web;

namespace $rootnamespace$
{
    public class RssFeed: HttpHandler<PageData>
    {       
        protected virtual string ContentType
        {
            get { return "application/rss+xml"; }
        }

        protected virtual Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            var feed = this.CreateSyndicationFeed();

            context.Response.ContentType = this.ContentType;
            context.Response.ContentEncoding = this.ContentEncoding;

            using (var xmlTextWriter = this.CreateXmlTextWriter(context))
            {
                feed.SaveAsRss20(xmlTextWriter);
            }                
        }

        protected virtual SyndicationFeed CreateSyndicationFeed()
        {
            var feedRootPage = this.CurrentPage ?? PageReference.StartPage.Resolve();

            var items = feedRootPage.GetChildren()
                .Select(CreateSyndicationItem)
                .Take(10);

            var feed = new SyndicationFeed(items)
                {
                    Title = new TextSyndicationContent(feedRootPage.PageName)
                };

            return feed;
        }

        protected virtual SyndicationItem CreateSyndicationItem(PageData pageData)
        {
            return new SyndicationItem
                {
                    Id = pageData.PageGuid.ToString(),
                    Title = new TextSyndicationContent(pageData.PageName),
                    PublishDate = pageData.StartPublish,
                    // Summary = new TextSyndicationContent(pageData.Intro),
                    // Content = new XhtmlSyndicationContent(pageData.MainBody)
                };
        }

        protected virtual XmlTextWriter CreateXmlTextWriter(HttpContext context)
        {
            return new XmlTextWriter(context.Response.OutputStream, this.ContentEncoding);
        }
    }
}