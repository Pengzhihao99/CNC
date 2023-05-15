using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.DataTransferModels
{
    public class TemplateResult
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 模板头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 模板尾
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// html 页面
        /// </summary>
        public string HtmlPage { get; set; }

        public TemplateResult(string subject, string header, string content, string footer, string htmlPage)
        {
            Subject = subject;
            Header = header;
            Content = content;
            Footer = footer;
            HtmlPage = htmlPage;
        }
    }
}
