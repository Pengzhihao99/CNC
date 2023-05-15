﻿using Framework.Application.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.DataTransferModels
{
    public class IssuerQuery : AbstractPagingFindQuery
    {
        public string? Name { get; set; }
    }
}
