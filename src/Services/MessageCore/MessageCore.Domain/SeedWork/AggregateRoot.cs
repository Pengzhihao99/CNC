using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.SeedWork
{
    public abstract class AggregateRoot : AggregateRootBase<string>
    {
        public AggregateRoot() 
        {
            CreateOn = DateTime.Now;
            UpdateOn = DateTime.Now;
        }

        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }

        public void SetUpdateOn()
        {
            this.UpdateOn = DateTime.Now;
        }
    }
}
