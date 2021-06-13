namespace DotnetDevPack.AspNetCore.Cors
{

  using System;
  using System.Collections.Generic;
  using DotnetDevPack.Utilities;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// 跨域配置
  /// </summary>
  public static class CorsExtensions
  {
    /// <summary>
    /// IServiceCollection扩展方法
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <returns>返回服务容器</returns>
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
      IConfiguration configuration = services.GetSingletonInstanceOrNull<IConfiguration>(); // 获取配置信息
      if(configuration is null)
      {
        throw new Exception("we");
      }

      var corsOrigin = configuration.GetSection("CorsOrigin").Get<List<string>>();
      if (corsOrigin != null && corsOrigin.Count > 0)
      {
        services.AddCors(options =>
        {
          options.AddDefaultPolicy(policy =>
          {
            policy.WithOrigins(corsOrigin.ToArray())
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
        });
      }

      return services;
    }

    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">ASP应用程序构建器</param>
    /// <returns>返回ASP应用程序构建器</returns>
    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
      if (app is null)
      {
        throw new ArgumentNullException(nameof(app));
      }

      app.UseCors();

      return app;
    }
  }
}
