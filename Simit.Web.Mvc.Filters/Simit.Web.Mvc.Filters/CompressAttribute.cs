namespace Simit.Web.Mvc.Filters
{
    using System.IO.Compression;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CompressAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(encodingsAccepted)) return;

            HttpResponseBase response = filterContext.HttpContext.Response;
            if (response.Filter == null) return;

            encodingsAccepted = encodingsAccepted.ToLowerInvariant();

            if (encodingsAccepted.Contains("gzip"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (encodingsAccepted.Contains("deflate"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}