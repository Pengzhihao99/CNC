using FluentValidation;
using Framework.Application.Core.Commands;
using MediatR;
using MessageCore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Base
{
    public class CommandValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : ICommand, IRequest<TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public CommandValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //如果存在对应的验证器，配合TRequest类型注入
            if (_validators != null && _validators.Any())
            {
                var failures = _validators
                    .Select(v => v.ValidateAsync(request))
                    .SelectMany(result => result.ConfigureAwait(false).GetAwaiter().GetResult().Errors)
                    .Where(error => error != null)
                    .ToList();

                if (failures.Any())
                {
                    var messages = new List<string>();
                    foreach (var failure in failures)
                    {
                        messages.Add(failure.ErrorMessage);
                    }
                    throw new MessageCoreInternalException(ErrorCode.StringCode.CommandValidateError, string.Format(ErrorMessage.CommandValidateError, string.Join(" ", messages)));
                }
            }
            var response = await next();
            return response;
        }
    }
}
