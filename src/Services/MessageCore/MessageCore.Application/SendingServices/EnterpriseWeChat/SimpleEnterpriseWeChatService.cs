using MessageCore.Application.DataTransferModels;
using MessageCore.Application.SendingServices.Base;
using MessageCore.Application.SendingServices.EnterpriseWeChat.Dto;
using MessageCore.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MessageCore.Application.SendingServices
{
    /// <summary>
    /// https://git.chukou1.com/bfe/weworkapi
    /// </summary>
    public class SimpleEnterpriseWeChatService : IEnterpriseWeChatService
    {
        private readonly ILogger<SimpleEnterpriseWeChatService> _logger;
        public SimpleEnterpriseWeChatService(ILogger<SimpleEnterpriseWeChatService> logger)
        {
            _logger = logger;
        }


        public async Task SendAsync(From from, To to, string content, Account account)
        {
            var touser = to.Address;
            var client = new RestClient(account.Host);
            const string resource = @"/sendmessage";
            var request = new RestRequest(resource, Method.Get);
            request.AddHeader("Authorization", "Bearer " + account.Password);
            request.AddParameter("touser", touser, ParameterType.GetOrPost);
            request.AddParameter("text", content, ParameterType.GetOrPost);

            //发起请求
            var responseData = await client.ExecuteAsync(request);

            if (responseData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response response = null;
                try
                {
                    response = JsonConvert.DeserializeObject<Response>(responseData.Content);
                }
                catch (Exception)
                {
                    ResponseError responseError = null;
                    try
                    {
                        responseError = JsonConvert.DeserializeObject<ResponseError>(responseData.Content);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }

                    if (responseError == null)
                    {
                        throw new MessageCoreExternalException(ErrorCode.StringCode.SimpleEnterpriseWeChatServiceError, string.Format(ErrorMessage.SimpleEnterpriseWeChatServiceError, responseData.Content));
                    }
                    else
                    {
                        throw new MessageCoreExternalException(ErrorCode.StringCode.SimpleEnterpriseWeChatServiceError, string.Format(ErrorMessage.SimpleEnterpriseWeChatServiceError, "发送企业微信异常:【errmsg】" + responseError.errmsg + "【invaliduser】" + responseError.invaliduser));
                    }
                }

                if (response == null)
                {
                    throw new MessageCoreExternalException(ErrorCode.StringCode.SimpleEnterpriseWeChatServiceError, string.Format(ErrorMessage.SimpleEnterpriseWeChatServiceError, responseData.Content));
                }
            }
            else
            {
                _logger.LogError("发送企业微信异常:" + responseData.StatusCode + " " + responseData.Content);
                throw new MessageCoreExternalException(ErrorCode.StringCode.SimpleEnterpriseWeChatServiceError, string.Format(ErrorMessage.SimpleEnterpriseWeChatServiceError, responseData.StatusCode));
            }
        }
    }
}
