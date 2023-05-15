using Framework.Application.Core.Commands;
using MediatR;
using MessageCore.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.BlockingCommand
{
    public class CreateBlockingCommand : IRequest, ICommand
    {
        public IList<string> Blacklists { get; set; }

        /// <summary>
        /// 模板名称（测试用，后面再考虑做不做）
        /// </summary>
        public string TemplateName { get; set; }
    }
}
