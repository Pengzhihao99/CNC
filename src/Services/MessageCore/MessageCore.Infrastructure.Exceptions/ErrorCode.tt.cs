using System;

namespace MessageCore.Infrastructure.Exceptions
{	
	public static class ErrorCode
    {
		public static class NumbericCode
		{
		    /// <summary>
			/// 系统出现未处理异常：{0}
			/// </summary>
			public const int UnhandledError = unchecked((int)0x800F8000);

		    /// <summary>
			/// 数据验证失败：{0}
			/// </summary>
			public const int CommandValidateError = unchecked((int)0x800F8001);

		    /// <summary>
			/// 找不到消息拦截数据：{0}
			/// </summary>
			public const int MessageBlockingNotFoundError = unchecked((int)0x800F8002);

		    /// <summary>
			/// 找不到发起人信息：{0}
			/// </summary>
			public const int IssuerNotFoundError = unchecked((int)0x800F8003);

		    /// <summary>
			/// 数据验证失败：{0}
			/// </summary>
			public const int ValidateError = unchecked((int)0x800F8004);

		    /// <summary>
			/// 找不到模板信息：{0}
			/// </summary>
			public const int TemplateNotFoundError = unchecked((int)0x800F8005);

		    /// <summary>
			/// 消息服务{0}未开启
			/// </summary>
			public const int SendingServiceEnabledError = unchecked((int)0x800F8006);

		    /// <summary>
			/// 找不到消息服务：{0}
			/// </summary>
			public const int MessageSendingServiceNotFoundError = unchecked((int)0x800F8007);

		    /// <summary>
			/// 用户：{0}，登录失败！{1}
			/// </summary>
			public const int LoginFail = unchecked((int)0x800F8008);

		    /// <summary>
			/// 发送信息服务类型错误！
			/// </summary>
			public const int SendingServiceTypeError = unchecked((int)0x800F8009);

		    /// <summary>
			/// 找不到邮件服务！
			/// </summary>
			public const int EmailServiceNotFound = unchecked((int)0x800F8010);

		    /// <summary>
			/// 找不到短信服务！
			/// </summary>
			public const int SmsServiceNotFound = unchecked((int)0x800F8011);

		    /// <summary>
			/// 找不到企业微信服务！
			/// </summary>
			public const int EnterpriseWeChatServiceNotFound = unchecked((int)0x800F8012);

		    /// <summary>
			/// 发送企业微信失败：{0}
			/// </summary>
			public const int SimpleEnterpriseWeChatServiceError = unchecked((int)0x800F8013);

		    /// <summary>
			/// 登出失败！{1}
			/// </summary>
			public const int LogoutFail = unchecked((int)0x800F8014);

		    /// <summary>
			/// 获取用户信息失败：{0}
			/// </summary>
			public const int GetUserInfoError = unchecked((int)0x800F8015);

		    /// <summary>
			/// 登陆失败：{0}
			/// </summary>
			public const int ValidateTokenError = unchecked((int)0x800F8016);

		}

		public static class StringCode
		{
			/// <summary>
			/// 系统出现未处理异常：{0}
			/// </summary>
			public const string UnhandledError = "800F8000";

			/// <summary>
			/// 数据验证失败：{0}
			/// </summary>
			public const string CommandValidateError = "800F8001";

			/// <summary>
			/// 找不到消息拦截数据：{0}
			/// </summary>
			public const string MessageBlockingNotFoundError = "800F8002";

			/// <summary>
			/// 找不到发起人信息：{0}
			/// </summary>
			public const string IssuerNotFoundError = "800F8003";

			/// <summary>
			/// 数据验证失败：{0}
			/// </summary>
			public const string ValidateError = "800F8004";

			/// <summary>
			/// 找不到模板信息：{0}
			/// </summary>
			public const string TemplateNotFoundError = "800F8005";

			/// <summary>
			/// 消息服务{0}未开启
			/// </summary>
			public const string SendingServiceEnabledError = "800F8006";

			/// <summary>
			/// 找不到消息服务：{0}
			/// </summary>
			public const string MessageSendingServiceNotFoundError = "800F8007";

			/// <summary>
			/// 用户：{0}，登录失败！{1}
			/// </summary>
			public const string LoginFail = "800F8008";

			/// <summary>
			/// 发送信息服务类型错误！
			/// </summary>
			public const string SendingServiceTypeError = "800F8009";

			/// <summary>
			/// 找不到邮件服务！
			/// </summary>
			public const string EmailServiceNotFound = "800F8010";

			/// <summary>
			/// 找不到短信服务！
			/// </summary>
			public const string SmsServiceNotFound = "800F8011";

			/// <summary>
			/// 找不到企业微信服务！
			/// </summary>
			public const string EnterpriseWeChatServiceNotFound = "800F8012";

			/// <summary>
			/// 发送企业微信失败：{0}
			/// </summary>
			public const string SimpleEnterpriseWeChatServiceError = "800F8013";

			/// <summary>
			/// 登出失败！{1}
			/// </summary>
			public const string LogoutFail = "800F8014";

			/// <summary>
			/// 获取用户信息失败：{0}
			/// </summary>
			public const string GetUserInfoError = "800F8015";

			/// <summary>
			/// 登陆失败：{0}
			/// </summary>
			public const string ValidateTokenError = "800F8016";

		}

        public static string ToHex(this int value)
        {
            return "0x" + value.ToString("X");
        }
    }
}
