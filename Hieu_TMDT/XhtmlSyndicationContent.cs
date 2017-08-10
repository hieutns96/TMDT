using System.ServiceModel.Syndication;
namespace Hieu_TMDT
{
    public class XhtmlSyndicationContent : TextSyndicationContent
    {
        public XhtmlSyndicationContent(string text)
            : base(text, TextSyndicationContentKind.XHtml)
        {
        }

        protected override void WriteContentsTo(System.Xml.XmlWriter writer)
        {
            writer.WriteRaw(this.Text.Replace("&nbsp;", "&#160;"));
        }
    }
}