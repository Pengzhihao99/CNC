using Framework.Application.Core.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.ThemeCommand
{
    public class CreateThemeCommand : IRequest, ICommand
    {
        /// <summary>
        /// 主题名字
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 订阅者类型
        /// </summary>
        public int SubscriberType { get; set; }

        /// <summary>
        /// 发送服务类型(用于限定信息模板)
        /// </summary>
        public int SendingServiceType { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 信息模板绑定
        /// </summary>
        public List<string> TemplateIds { get; set; }

        /// <summary>
        /// 订阅者信息
        /// </summary>
        public List<string> SubscriberIds { get; set; }
    }
}
