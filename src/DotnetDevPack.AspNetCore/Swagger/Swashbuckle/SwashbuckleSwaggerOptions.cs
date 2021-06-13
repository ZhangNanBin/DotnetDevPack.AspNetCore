namespace DotnetDevPack.AspNetCore.Swagger.Swashbuckle
{
  using global::Swashbuckle.AspNetCore.SwaggerUI;
  using Microsoft.OpenApi.Models;

  public class SwashbuckleSwaggerOptions : SwaggerOptions
  {

    /// <summary>
    /// 获取或设置 Url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// OpenApiOAuthFlow
    /// </summary>
    public OpenApiOAuthFlow OpenApiOAuthFlow { get; set; }

    /// <summary>
    /// 客户端设置
    /// </summary>
    public OAuthConfigObject OAuth2ClientSettings { get; set; }
  }
}
