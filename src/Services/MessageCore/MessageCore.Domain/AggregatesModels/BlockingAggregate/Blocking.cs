using Framework.Domain.Core.Entities;
using MessageCore.Domain.SeedWork;

namespace MessageCore.Domain.AggregatesModels.BlockingAggregate
{
    /// <summary>
    /// 
    /// </summary>
    public class Blocking : AggregateRoot
    {
        public IList<string> Blacklists { get; private set; }

        public string TemplateName { get; set; }

        public Blocking(string templateName, IList<string> blacklists)
        {
            SetBlacklists(blacklists);
            TemplateName = templateName;
        }

        public void SetBlacklists(IList<string> blacklists)
        {
            if (blacklists != null && blacklists.Count > 0)
            {
                Blacklists = blacklists.Distinct().ToList();
            }
        }
    }
}