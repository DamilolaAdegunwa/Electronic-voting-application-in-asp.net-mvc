using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Evote.Models.Helpers
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Shows Validation summary in style
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlString BootstrapValidationSummary(this HtmlHelper html)
        {
            if (html == null)
                return new HtmlString("");

            var sb = new StringBuilder();

            if (!html.ViewData.ModelState.IsValid)
            {
                sb.Append("<div class='alert alert-danger'>");
                sb.Append(html.ValidationSummary());
                sb.Append("</div>");
            }
            return new HtmlString(sb.ToString());
        }
        /// <summary>
        /// Displays any Temporary message stored in "TempData"
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlString ShowFlashMessage(this HtmlHelper html, string key = "Message", string @class = "success")
        {
            var sb = new StringBuilder();
            if (html.ViewContext.TempData.ContainsKey(key))
            {

                @class = (string)html.ViewContext.TempData["MessageType"] ?? @class;


                sb.Append($"<br /><div class='alert alert-{@class}'>");
                if (@class == "success")
                {
                    sb.Append($"<h1><i class='icon fa fa-check'></i>Success!</h1>");
                }
                else if (@class == "danger")
                {
                    sb.Append($"<h1><i class='icon fa fa-ban'></i>Error!</h1>");
                }
                else if (@class == "warning")
                {
                    sb.Append($"<h1><i class='glyphicon glyphicon-warning-sign'></i>Warning!</h1>");
                }
                else if (@class == "info")
                {
                    sb.Append($"<h1><i class='icon fa fa-info'></i>Info!</h1>");
                }
                sb.Append($"<p>{html.ViewContext.TempData[key]}</p>");
                sb.Append("</div>");
            }

            return new HtmlString(sb.ToString());
        }
    }
}