using PaperFormatDetection.Models;
using System;
using System.Web.Mvc;


namespace PaperFormatDetection.Helper
{
    public static class PagingHelper
    {
        //HtmlHelper扩展方法，用于分页
        public static MvcHtmlString Pagination(this HtmlHelper html, PagingInfo pageInfo, Func<PagingInfo, string> pageLinks)
        {
            var htmlString = pageLinks(pageInfo);

            return MvcHtmlString.Create(htmlString);
        }
    }
}