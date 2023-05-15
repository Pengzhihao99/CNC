using FluentValidation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace MessageCore.Application.OpenApi.Commands.OrderCommand.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            //RuleFor(command => command).NotNull().WithMessage("CreateOrderCommand is required.");
            RuleFor(command => command.ReferenceNumber).NotEmpty().WithMessage("CreateOrderCommand.ReferenceNumber is required.");

            RuleFor(command => command.TemplateName).NotNull().WithMessage("CreateOrderCommand.TemplateName is required.");

            RuleFor(command => command.Sender).NotEmpty().WithMessage("CreateOrderCommand.Sender is required.");

            RuleFor(command => command.Token).NotEmpty().WithMessage("CreateOrderCommand.Token is required.");

            RuleFor(command => command.Attachments).Custom((list, context) =>
            {
                if (list != null && list.Count > 0)
                {
                    if (list.Any(item => string.IsNullOrWhiteSpace(item.Name)))
                    {
                        context.AddFailure($"CreateOrderCommand.Attachments.Name can not be Empty.");
                    }

                    if (list.Any(item => item.Data == null || item.Data.Length < 1))
                    {
                        context.AddFailure($"CreateOrderCommand.Attachments.Data can not be Empty.");
                    }

                    if (list.Any(item => string.IsNullOrWhiteSpace(item.AttachmentType)))
                    {
                        context.AddFailure($"CreateOrderCommand.Attachments.AttachmentType can not be Empty.");
                    }
                }
            });

            RuleFor(command => command).Custom((command, context) =>
            {
                if (command == null)
                {
                    context.AddFailure("CreateOrderCommand is required.");
                }

                if (!command.TemplateName.Equals("GenericEmailTemplate") && !command.TemplateName.Equals("GenericWeChatTemplate"))
                {
                    if (command.Content != null)
                    {
                        try
                        {
                            JObject.Parse(JsonConvert.SerializeObject(command.Content));
                        }
                        catch (Exception)
                        {
                            context.AddFailure($"CreateOrderCommand.Content must be a JSON-serializable object.");
                        }
                    }
                }
            });


            //暂时添加
            RuleFor(command => command.Receivers).Custom((receivers, context) =>
            {
                if (receivers == null || receivers.Count < 1)
                {
                    context.AddFailure($"CreateOrderCommand.receivers is required.");
                }
                else if (receivers != null && receivers.Count > 1)
                {
                    foreach (var receiver in receivers)
                    {
                        if (string.IsNullOrWhiteSpace(receiver.EnterpriseWeChat) && string.IsNullOrWhiteSpace(receiver.Email))
                        {
                            context.AddFailure($"CreateOrderCommand.receivers Phone,EnterpriseWeChat,Email Cannot be both empty");
                        }
                    }
                }
            });
        }
    }
}
