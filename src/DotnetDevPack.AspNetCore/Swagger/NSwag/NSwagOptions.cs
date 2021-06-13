namespace DotnetDevPack.AspNetCore.Swagger.NSwag
{
  using global::NSwag;
  using global::NSwag.AspNetCore;

  public class NSwagOptions : SwaggerOptions
  {
    /// <summary>
    /// OpenApiOAuthFlow
    /// </summary>
    public OpenApiOAuthFlow OpenApiOAuthFlow { get; set; }

    /// <summary>
    /// 客户端设置
    /// </summary>
    public OAuth2ClientSettings OAuth2ClientSettings { get; set; }
  }
}
