using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.ViewModels
{
    public class BlockingVM
    {
        public string Id { get; set; }

        public IList<string> Blacklists { get; set; }

        public string TemplateName { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}
