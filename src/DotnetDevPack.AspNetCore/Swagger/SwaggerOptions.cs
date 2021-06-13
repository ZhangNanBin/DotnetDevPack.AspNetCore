namespace DotnetDevPack.AspNetCore.Swagger
{
  using System.Collections.Generic;

  /// <summary>
  /// Swagger选项
  /// </summary>
  public class SwaggerOptions
  {
    /// <summary>
    /// 使用Swagger类型
    /// </summary>
    /// <value></value>
    public SwaggerType SwaggerType { get; set; }
    
    /// <summary>
    /// 获取或设置 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 获取或设置 版本
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 安全方案
    /// </summary>
    public SecurityScheme SecurityScheme { get; set; }

    /// <summary>
    /// 获取或设置 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 客户端作用域
    /// </summary>
    public List<KeyValueScope> Scopes { get; set; }

    /// <summary>
    /// 客户端作用域Value
    /// </summary>
    public IDictionary<string, string> ScopeValues
    {
      get
      {
        var value = new Dictionary<string, string>();
        foreach (var scope in Scopes)
        {
          value.Add(scope.Key, scope.Value);
        }

        return value;
      }
    }

    /// <summary>
    /// 键值对
    /// </summary>
    public class KeyValueScope
    {
      public string Key { get; set; }
      public string Value { get; set; }
    }
  }
}
