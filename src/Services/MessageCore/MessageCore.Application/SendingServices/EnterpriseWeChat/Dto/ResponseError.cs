using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.SendingServices.EnterpriseWeChat.Dto
{
    public class ResponseError
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string invaliduser { get; set; }
        public string msgid { get; set; }
    }
}
