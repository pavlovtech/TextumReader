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
            Func<int, string> pageUrl, int amountOfPages)
        {
            if (pagingInfo.TotalItems == 1)
            {
                return MvcHtmlString.Create("");
            }

            StringBuilder result = new StringBuilder();

            var navBack = BuildNavItem(pagingInfo, pageUrl, Direction.Forward);
            result.Append(navBack);

            if (pagingInfo.CurrentPage != 1 && pagingInfo.CurrentPage != 2)
            {
                result.Append(CreateItem(pageUrl, 1));

                if (pagingInfo.CurrentPage != 3)
                result.Append("<li><a>...</a></li>");
            }

            int startIndex;

            if (pagingInfo.CurrentPage == 1)
                startIndex = 1;
            else
                startIndex = pagingInfo.CurrentPage - 1;

            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                startIndex = pagingInfo.TotalPages - amountOfPages;


            for (int i = startIndex; i <= startIndex + amountOfPages; i++)
            {
                var item = CreateItem(pageUrl, i);

                if (i == pagingInfo.CurrentPage)
                    item.AddCssClass("active");

                result.Append(item.ToString());
            }

            if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
            {
                result.Append("<li><a>...</a></li>");

                result.Append(CreateItem(pageUrl, pagingInfo.TotalPages).ToString());
            }

            var navForward = BuildNavItem(pagingInfo, pageUrl, Direction.Back);
            result.Append(navForward);
            return MvcHtmlString.Create(result.ToString());
        }

        private static TagBuilder CreateItem(Func<int, string> pageUrl, int pageNumber)
        {
            TagBuilder select = new TagBuilder("li");
            TagBuilder tag = new TagBuilder("a");

            tag.MergeAttribute("href", pageUrl(pageNumber));
            tag.InnerHtml = pageNumber.ToString();

            @select.InnerHtml = tag.ToString();
            return @select;
        }

        private static string BuildNavItem(PagingInfo pagingInfo, Func<int, string> pageUrl, Direction direction)
        {
            TagBuilder navLi = new TagBuilder("li");
            TagBuilder navA = new TagBuilder("a");

            if (direction == Direction.Forward)
            {
                if (pagingInfo.CurrentPage != 1)
                    navA.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                navA.InnerHtml = "&laquo;";
            }
            if (direction == Direction.Back)
            {
                if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
                    navA.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
                navA.InnerHtml = "&raquo;";
            }

            navLi.InnerHtml = navA.ToString();
            return navLi.ToString();
        }

        private enum Direction
        {
            Back,
            Forward
        }
    }
}