using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.ValueObjects
{
    /// <summary>
    /// 模板详细内容
    /// </summary>
    public class TemplateInfo : ValueObject
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

        ///// <summary>
        ///// 模板字段值
        ///// </summary>
        //public object FieldValue { get; set; }

        protected TemplateInfo() 
        {

        }

        public TemplateInfo(string subject, string header, string content, string footer)
        {
            Subject = subject;
            Header = header;
            Content = content;
            Footer = footer;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Subject;
            yield return Header;
            yield return Content;
            yield return Footer;
        }
    }
}
