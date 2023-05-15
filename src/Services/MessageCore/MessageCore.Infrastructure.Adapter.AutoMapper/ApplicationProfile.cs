using AutoMapper;
using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;

namespace MessageCore.Infrastructure.Adapter.AutoMapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            this.CreateMap<Blocking, BlockingVM>();
            this.CreateMap<PagedResultWithCount<Blocking>, PagingFindResultWrapper<BlockingVM>>();

            this.CreateMap<Issuer, IssuerVM>();
            this.CreateMap<PagedResultWithCount<Issuer>, PagingFindResultWrapper<IssuerVM>>();

            this.CreateMap<SendingService, SendingServiceVM>();
            this.CreateMap<SendingService, SendingServiceNamesVM>();
            //this.CreateMap<IList<SendingService>, List<SendingServiceNamesVM>>();
            this.CreateMap<PagedResultWithCount<SendingService>, PagingFindResultWrapper<SendingServiceVM>>();

            this.CreateMap<Subscriber, SubscriberVM>();
            this.CreateMap<PagedResultWithCount<Subscriber>, PagingFindResultWrapper<SubscriberVM>>();
            
            this.CreateMap<Template, TemplateVM>();
            // .ForMember(dest => dest.TimerType, option => option.MapFrom(sorc => sorc.Id))
            this.CreateMap<PagedResultWithCount<Template>, PagingFindResultWrapper<TemplateVM>>();

            this.CreateMap<SendingOrder, SendingOrderVM>()
                .ForMember(dest => dest.TimerType, option => option.MapFrom(sorc => sorc.TimerType.Name))
                .ForMember(dest => dest.SendingOrderStatus, option => option.MapFrom(sorc => sorc.SendingOrderStatus.Name));
            this.CreateMap<PagedResultWithCount<SendingOrder>, PagingFindResultWrapper<SendingOrderVM>>();
        }
    }
}
