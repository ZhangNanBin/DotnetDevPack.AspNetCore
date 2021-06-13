namespace DotnetDevPack.AspNetCore.Swagger
{
  using System.ComponentModel;

  /// <summary>
  /// Swagger安全方案
  /// </summary>
  public enum SecurityScheme
  {
    /// <summary>
    /// JWT
    /// </summary>
    [Description("JWT")]
    JWT = 0,

    /// <summary>
    /// OAuth2
    /// </summary>
    [Description("OAuth2")]
    OAuth2 = 1
  }
}
