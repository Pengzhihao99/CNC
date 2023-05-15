系统简码：CNC<br />
系统简称：NotificationCenter<br />
系统全称：出口易消息中心管理系统<br />

# OpenApi及SDK包调用说明：<br />

出口易消息中心管理系统登录：Https://cnc.chukou1.cn/#/login<br />

## SDK包说明：
SDK包：MessageCore.OpenApi.SDK<br />
控制台下载命令：Install-Package MessageCore.OpenApi.SDK -Version 1.0.1 -Source https://nexus.chukou1.com/nuget/chukou1-nuget/v3/index.json<br />
SDK发送消息订单服务：MessagerOrderService<br />
SDK发送消息订单方法：CreateMessagerOrderAsync，CreateSimpleEmailOrder(1.0.1版本新增)，CreateSimpleEnterpriseWeChatOrder(1.0.1版本新增)，请下载最新版本使用！<br />
调用示例： <br />
```
var result = await new MessagerOrderService("https://cnc-openapi.ck1info.com").CreateMessagerOrderAsync(jsonObj);
```


## BaseUrl:<br />
正式环境：https://cnc-openapi.chukou1.cn<br />
测试环境：https://cnc-openapi.ck1info.com<br />
POST: api/v1/orders<br />

## OpenApi接口及SDK包DTO参数说明：

| 参数   |      是否必须      |  说明 |
|----------|:-------------:|:------|
| referenceNumber |  是 | 参考号，每次请求唯一。|
| templateName |    是   |   模板名，目前管理端设有用邮件模板，模板名为'GenericEmailTemplate'；通用企业微信模板；模板名为'GenericWeChatTemplate'。|
| content | 是 |    消息内容，格式为JSON，目前通用模板包含参数subject、header、content、footer四个参数，如需使用其他模板请在管理端添加，内容参数字段可自定义。|
| sender |  是 | 发起者，请登录管理端系统查看或添加。|
| token |  是 | token凭证，请登录管理端系统查看或添加。|
| receivers |  是 | 收件人，包含参数收件人名字'name'、邮箱'Email'、手机'phone'、企业微信'enterpriseWeChat'(填写员工号)。|
| attachments |  否 | 附件，包含参数data，格式为Base64。文件名name需包含后缀，格式如："测试.pdf"。文件类型AttachmentType建议使用MessageCore.OpenApi.SDK.Definitions.Constant.AttachmentType赋值。|
| group |  否 | 组，客户代码，暂无用处。|

## 创建标准模板信息请求示例：<br />
**PS**：示例token值为测试环境所设，调用时请使用https://cnc-openapi.ck1info.com
```
{
    "referenceNumber": "Ref123123123",
    "templateName": "GenericWeChatTemplate",
    "content": {
        "Subject": "包裹",
        "Header": "测试人员",
        "Content": "特别提醒",
        "Footer": "BFE"
    },
    "sender":"SRM",
    "token":"94e7012529134a67a83a140255d76ced",
    "receivers": [
        {
            "name": "张三",
            "email": "san.zhang@chukou1.com",
            "phone": "12345678910",
            "enterpriseWeChat": "12345"
        }
    ],
    "attachments": [
        {
            "data": "base64",
            "name": "测试标签.pdf",
            "AttachmentType": "PDF"
        }
    ],
    "group": "A"
}
```
## 创建简单企业微信消息订单请求示例：<br />
```
{
    "referenceNumber":"Ref20230511002",
    "content":"特别提醒",
    "sender":"SRM",
    "token":"94e7012529134a67a83a140255d76ced",
    "receivers":[
        "12345"
    ]
}
```

## 创建简单邮件消息订单请求示例：<br />
```
{
    "referenceNumber":"Ref20230511001",
    "subject":"测试",
    "content":"特别提醒",
    "sender":"SRM",
    "token":"94e7012529134a67a83a140255d76ced",
    "receivers":[
        "test@chukou1.com"
    ]
}
```
```
支持附件：
{
    "referenceNumber":"Ref20230511001",
    "subject":"测试",
    "content":"特别提醒",
    "sender":"SRM",
    "token":"94e7012529134a67a83a140255d76ced",
    "receivers":[
        "test@chukou1.com"
    ],
    "attachments":[
        {
            "data":"base64",
            "name":"测试标签.pdf",
            "AttachmentType":"PDF"
        }
    ]
}
```

## 响应示例：<br />
200 无返回内容<br />
400
```
{
    "TicketId": "38e7f559-489c-4d7e-9733-911b867d0dc5",
    "UtcDateTime": "2023-05-08T09:37:20.9628309Z",
    "RequestUri": "http://cnc-openapi.chukou1.cn/api/v1/orders",
    "Errors": [
        {
            "Sn": null,
            "Code": "000000",
            "Message": "referenceNumber is repeat!Ref20230401311a21"
        }
    ]
}
```
## 注意事项：<br />
1.系统会根据管理端消息模板区分邮件发送和企业微信发送两种方式，企业微信发送请在enterpriseWeChat字段填写员工号。<br />
2.调用系统的通知消息首先会进入采集表，之后进行过滤拦截操作后，由定时器定时发起推送，暂时上线推送方式为一分钟执行一次，重试机制为一小时执行一次。<br />
3.SDK包调用，需在构造函数传参BaseUrl。<br />

## 模板引擎组件：[Scriban](https://github.com/scriban/scriban/)
ASP.NET Core Sample：https://scribanonline.azurewebsites.net/<br />
组件文档：https://github.com/scriban/scriban/blob/master/doc/language.md