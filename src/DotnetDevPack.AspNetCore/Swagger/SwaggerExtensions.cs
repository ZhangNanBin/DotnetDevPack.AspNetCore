namespace DotnetDevPack.AspNetCore.Swagger
{
  using System;
  using DotnetDevPack.Utilities;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Options;

  /// <summary>
  /// Swagger配置
  /// </summary>
  public static class SwaggerExtensions
  {
    /// <summary>
    /// IServiceCollection扩展方法
    /// 添加自定义的Swagger
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <returns>返回服务容器</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string appSwaggerSettingsKey = "Swagger")
    {
      IConfiguration configuration = services.GetSingletonInstanceOrNull<IConfiguration>(); // 获取配置信息

      var swaggerOptionsSection = configuration.GetSection(appSwaggerSettingsKey);
      services.Configure<SwaggerOptions>(swaggerOptionsSection);
      SwaggerOptions swaggerOptions = swaggerOptionsSection.Get<SwaggerOptions>();

      if (swaggerOptions == null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      if (swaggerOptions.SwaggerType == SwaggerType.NSwag)
      {
        return NSwag.NSwagExtensions.AddCustomSwagger(services, appSwaggerSettingsKey);
      }
      else if (swaggerOptions.SwaggerType == SwaggerType.Swashbuckle)
      {
        return Swashbuckle.SwashbuckleSwaggerExtensions.AddCustomSwagger(services, appSwaggerSettingsKey);
      }

      return services;
    }

    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">ASP应用程序构建器</param>
    /// <returns>返回ASP应用程序构建器</returns>
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
      if (app is null)
      {
        throw new ArgumentNullException(nameof(app));
      }

      IConfiguration configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
      SwaggerOptions swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;

      if (swaggerOptions == null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      if (swaggerOptions.SwaggerType == SwaggerType.NSwag)
      {
        return NSwag.NSwagExtensions.UseCustomSwagger(app);
      }
      else if (swaggerOptions.SwaggerType == SwaggerType.Swashbuckle)
      {
        return Swashbuckle.SwashbuckleSwaggerExtensions.UseCustomSwagger(app);
      }

      return app;
    }
  }
}
