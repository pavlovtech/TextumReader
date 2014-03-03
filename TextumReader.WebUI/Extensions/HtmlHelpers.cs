using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TextumReader.WebUI.Extensions
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            string result;

            if (controllerName == currentController && actionName == currentAction)
                result = String.Format("<li class='nav active'>{0}</li>",
                htmlHelper.ActionLink(linkText, actionName, controllerName).ToString());
            else 
            result = String.Format("<li class='nav'>{0}</li>",
                htmlHelper.ActionLink(linkText, actionName, controllerName).ToString());

            return new MvcHtmlString(result);
        }
    }
}