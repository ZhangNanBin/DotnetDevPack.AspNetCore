# 描述

## 使用DotnetDevPack.AspNetCore.Cors配置


```
{
  "CorsOrigin": ["url","url"]
}
```
### CorsOrigin参数描述
* CorsOrigin ：跨域源，类型为字符串数组;

## 使用DotnetDevPack.AspNetCore.Swagger配置

### 配置
```
{
  "Swagger": {
    "SwaggerType": 0,
    "Title": "IdentityServer（配置管理服务）",
    "Version": "V1.0",
    "SecurityScheme": 1,
    "Description": "作者：Zhang_N_B<br/>邮箱：<a href=\"mailto:Zhang_N_@outlook.com\">Zhang_N_B@outlook.com</a> 或 <a href=\"mailto:Zhang_N_@qq.com\">Zhang_N_B@qq.com</a>",
    "Scopes": [
      {
        "Key": "IdentityService",
        "Value": "IdentityService配置服务"
      }
    ],
    "OpenApiOAuthFlow": {
      "AuthorizationUrl": "https://localhost:5000/connect/authorize",
      "TokenUrl": "https://localhost:5000/connect/token",
      "RefreshUrl": "https://localhost:5000/connect/token"
    },
    "OAuth2ClientSettings": {
      "ClientId": "IdentityService",
      "ClientSecret": "zhangyang",
      "Realm": "realm",
      "AppName": "IdentiService配置",
      "ScopeSeparator": " ",
      "UsePkceWithAuthorizationCodeGrant": true
    },
    "Url": "/swagger/V1.0/swagger.json",
  }
}
```
#### Swagger参数描述 
* SwaggerType ：Swagger类型；类型为SwaggerType（int）；0（NSwag）、1（Swashbuckle）
* Title ：标题，类型为字符串；如（Swagger配置）
* Version ：版本，类型为字符串；如（V1）
* Scopes ：客户端作用域，类型KeyValueScope
* SecurityScheme ：安全方案；类型为SecurityScheme（int）；0（JWT）、1（OAth2）
* Description ：描述，类型为字符串
* OpenApiOAuthFlow ：OAth配置 // 类型不一致参数一致
  - AuthorizationUrl ：鉴权URL，类型为字符串  
  - TokenUrl ：TokenURL，类型为字符串  
  - RefreshUrl ：刷新TokenURL，类型为字符串 
* OAuth2ClientSettings ：OAth客户端配置 // 类型不一致参数一致
  - ClientId ：客户端Id，类型为字符串  
  - ClientSecret ：客户端机密，类型为字符串  
  - Realm ：Realm，类型为字符串  
  - AppName ：App名称，类型为字符串  
  - ScopeSeparator ：作用域分隔符，类型为字符串  
  - UsePkceWithAuthorizationCodeGrant ：将Pkce与授权码一起使用，类型为布尔
* Url ：Swagger.Json文件URL，类型为字符串,SwaggerType为Swashbuckle使用；
