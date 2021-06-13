namespace DotnetDevPack.AspNetCore.Filters
{
  using System;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;

  /// <summary>
  /// SecurityHeadersAttributes
  /// HTTP头部安全设置,用于MVC程序
  /// </summary>
  public class SecurityHeadersAttribute : ActionFilterAttribute
  {
    /// <summary>
    /// OnResultExecuting
    /// </summary>
    /// <param name="context">ResultExecutingContext</param>
    public override void OnResultExecuting(ResultExecutingContext context)
    {
      if (context is null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var result = context.Result;
      if (result is ViewResult)
      {
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
        {
          context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
        {
          context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts allow-popups; base-uri 'self';";

        // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
        // csp += "upgrade-insecure-requests;";
        // also an example if you need client images to be displayed from twitter
        // csp += "img-src 'self' https://pbs.twimg.com;";
        csp += "img-src 'self' data: ;"; // 图片加载支持 自己(同源)和data:

        // once for standards compliant browsers
        if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
        {
          context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
        }

        // and once again for IE
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
        {
          context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
        }

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        var referrer_policy = "no-referrer";
        if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
        {
          context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
        }
      }
    }
  }
}
