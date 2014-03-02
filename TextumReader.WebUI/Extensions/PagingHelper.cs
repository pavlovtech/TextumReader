using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TextumReader.WebUI.Models;

namespace TextumReader.WebUI.Extensions
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder select = new TagBuilder("li");  
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                select.InnerHtml = tag.ToString();
                if (i == pagingInfo.CurrentPage)
                    select.AddCssClass("active");

                result.Append(select.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}