using Framework.Domain.Core.Entities;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Commands.ThemeCommand.Handlers
{
    public class CreateThemeCommandHandler : AsyncRequestHandler<CreateThemeCommand>
    {
        private readonly IThemeRepository _themeRepository;

        public CreateThemeCommandHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        protected async override Task Handle(CreateThemeCommand request, CancellationToken cancellationToken)
        {
            var theme = new Theme(request.ThemeName, request.LoginName, Enumeration.FromValue<SubscriberType>(request.SubscriberType),
                request.Enabled, request.TemplateIds, request.SubscriberIds, Enumeration.FromValue<SendingServiceType>(request.SendingServiceType));

            await _themeRepository.AddAsync(theme);
        }
    }
}
