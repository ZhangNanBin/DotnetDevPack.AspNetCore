namespace DotnetDevPack.AspNetCore.Filters
{
  using System;
  using DotnetDevPack.Exceptions;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;
  using Microsoft.Extensions.Logging;

  /// <summary>
  /// HTTP全局异常捕获
  /// </summary>ExceptionAttribute
  public class HttpGlobalExceptionAttribute : ExceptionFilterAttribute
  {
    private readonly ILogger<HttpGlobalExceptionAttribute> logger;

    /// <summary>
    /// HttpGlobalExceptionFilter
    /// </summary>
    /// <param name="env">环境变量</param>
    /// <param name="logger">日志</param>
    public HttpGlobalExceptionAttribute(ILogger<HttpGlobalExceptionAttribute> logger)
    {
      this.logger = logger;
    }

    /// <summary>
    /// OnException
    /// </summary>
    /// <param name="context">ExceptionContext</param>
    public override void OnException(ExceptionContext context)
    {
      if (context is null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      if (!context.ExceptionHandled)
      {
        bool isHttpGlobalException = context.Exception.GetType() == typeof(HttpGlobalException);
        if (!isHttpGlobalException)
        {
          logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);
        }

        var json = new JsonErrorResponse
        {
          Messages = isHttpGlobalException ? context.Exception.Message
            : "发生错误，请重试。多次错误后请联系管理员。错误原因：" + context.Exception.Message,
#if DEBUG
          DeveloperMessage = context.Exception.StackTrace
#endif
        };

        context.HttpContext.Response.StatusCode = isHttpGlobalException ?
          (int)((HttpGlobalException)context.Exception).StatusCode :
          StatusCodes.Status500InternalServerError;

        context.Result = new ObjectResult(json);
        context.ExceptionHandled = true;
      }
    }

    private class JsonErrorResponse
    {
      public string Messages { get; set; }

      public string DeveloperMessage { get; set; }
    }
  }
}
