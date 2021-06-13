namespace DotnetDevPack.AspNetCore.Swagger.NSwag
{
  using System;
  using System.Linq;
  using DotnetDevPack.Utilities;
  using global::NSwag;
  using global::NSwag.Generation.Processors.Security;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Options;

  /// <summary>
  /// Swagger扩展
  /// </summary>
  public static class NSwagExtensions
  {
    /// <summary>
    /// IServiceCollection扩展方法
    /// 添加自定义的Swagger
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <returns>返回服务容器</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string appSwaggerSettingsKey = "Swagger")
    {
      if (services is null)
      {
        throw new ArgumentNullException(nameof(services));
      }

      IConfiguration configuration = services.GetSingletonInstanceOrNull<IConfiguration>(); // 获取配置信息

      var swaggerOptionsSection = configuration.GetSection(appSwaggerSettingsKey);
      services.Configure<NSwagOptions>(swaggerOptionsSection);
      NSwagOptions swaggerOptions = swaggerOptionsSection.Get<NSwagOptions>();

      if (swaggerOptions is null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      services.AddOpenApiDocument(options =>
      {
        options.DocumentName = swaggerOptions.Version;
        options.PostProcess = d =>
        {
          d.Info.Title = swaggerOptions.Title;
          d.Info.Version = swaggerOptions.Version;
          d.Info.Description = swaggerOptions.Description;
          d.Schemes = new[] { OpenApiSchema.Http, OpenApiSchema.Https };
        };

        if (swaggerOptions.SecurityScheme == SecurityScheme.OAuth2)
        {
          // 使用OAuth2的方式认证
          options.AddSecurity("OAuth2", new OpenApiSecurityScheme
          {
            Type = OpenApiSecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
              AuthorizationCode = new OpenApiOAuthFlow
              {
                AuthorizationUrl = swaggerOptions.OpenApiOAuthFlow.AuthorizationUrl,
                TokenUrl = swaggerOptions.OpenApiOAuthFlow.TokenUrl,
                RefreshUrl = swaggerOptions.OpenApiOAuthFlow.RefreshUrl,
                Scopes = swaggerOptions.ScopeValues // 填写的是ApiScops
              }
            }
          });

          options.OperationProcessors.Add(new OperationSecurityScopeProcessor("OAuth2"));
        }
        else if (swaggerOptions.SecurityScheme == SecurityScheme.JWT)
        {
          // 使用JWT的方式认证
          options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
          {
            Type = OpenApiSecuritySchemeType.ApiKey,
            In = OpenApiSecurityApiKeyLocation.Header,
            Name = "Authorization",
            Description = "Type into the textbox: Bearer {your JWT token}."
          });

          options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        }
      });

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

      NSwagOptions swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<NSwagOptions>>().Value;

      if (swaggerOptions == null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      app.UseOpenApi();
      app.UseSwaggerUi3(options =>
      {
        if (swaggerOptions.SecurityScheme == SecurityScheme.OAuth2)
        {
          options.OAuth2Client = swaggerOptions.OAuth2ClientSettings;
        }
      });

      return app;
    }
  }
}
