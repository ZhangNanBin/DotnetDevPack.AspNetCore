namespace DotnetDevPack.AspNetCore.Swagger.Swashbuckle
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using DotnetDevPack.Utilities;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Options;
  using Microsoft.OpenApi.Models;

  public static class SwashbuckleSwaggerExtensions
  {
    /// <summary>
    /// IServiceCollection扩展方法
    /// 添加自定义的Swagger
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <param name="appSwaggerSettingsKey">配置文件节点</param>
    /// <returns>返回服务容器</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string appSwaggerSettingsKey = "Swagger")
    {
      if (services is null)
      {
        throw new ArgumentNullException(nameof(services));
      }

      IConfiguration configuration = services.GetSingletonInstanceOrNull<IConfiguration>(); // 获取配置信息
      var swaggerOptionsSection = configuration.GetSection(appSwaggerSettingsKey);
      services.Configure<SwashbuckleSwaggerOptions>(swaggerOptionsSection);
      SwashbuckleSwaggerOptions swaggerOptions = swaggerOptionsSection.Get<SwashbuckleSwaggerOptions>();

      if (swaggerOptions == null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      services.AddSwaggerGen(options =>
      {
        // 配置Swagger标题和版本信息
        options.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo
        {
          Title = swaggerOptions.Title,
          Version = swaggerOptions.Version,
          Description = swaggerOptions.Description
        });

        // 配置XML文档
        Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
        {
          options.IncludeXmlComments(file, true);
        });

        if (swaggerOptions.SecurityScheme == SecurityScheme.JWT)
        {
          // 使用JWT的方式认证
          options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
          {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header, // JWT默认存放Authorization信息的位置(请求头中)
            Name = "Authorization",
            Description = "Type into the textbox: Bearer {your JWT token}."
          });

          options.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
              },
              new List<string>()
            }
          });
        }
        else if (swaggerOptions.SecurityScheme == SecurityScheme.OAuth2)
        {
          // 使用OAuth2的方式认证
          options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
          {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
              AuthorizationCode = new OpenApiOAuthFlow
              {
                AuthorizationUrl = swaggerOptions.OpenApiOAuthFlow.AuthorizationUrl,
                TokenUrl = swaggerOptions.OpenApiOAuthFlow.TokenUrl,
                RefreshUrl = swaggerOptions.OpenApiOAuthFlow.RefreshUrl,
                Scopes = swaggerOptions.ScopeValues
              },
            }
          });

          options.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "OAuth2"
                },
                Scheme = "OAuth2",
                Name = "OAuth2",
                In = ParameterLocation.Header
              },
              new List<string>()
            }
          });
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

      IConfiguration configuration = app.ApplicationServices.GetRequiredService<IConfiguration>(); // 获取配置信息
      SwashbuckleSwaggerOptions swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwashbuckleSwaggerOptions>>().Value;

      if (swaggerOptions == null)
      {
        throw new Exception("Swagger配置不能为空");
      }

      Action<global::Swashbuckle.AspNetCore.Swagger.SwaggerOptions> setupAction = null;
      app.UseSwagger(setupAction);
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint(swaggerOptions.Url, $"{swaggerOptions.Title} {swaggerOptions.Version}");
        options.OAuthConfigObject = swaggerOptions.OAuth2ClientSettings;
      });

      return app;
    }
  }
}
