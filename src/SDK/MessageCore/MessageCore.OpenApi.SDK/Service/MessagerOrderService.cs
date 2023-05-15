using MessageCore.OpenApi.SDK.Definitions;
using MessageCore.OpenApi.SDK.Definitions.Common;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.OpenApi.SDK.Service
{
    /// <summary>
    /// 消息订单
    /// </summary>
    public class MessagerOrderService
    {
        public readonly string BaseUrl;

        /// <summary>
        /// URL构造函数
        /// </summary>
        /// <param name="baseurl">域名URL</param>
        public MessagerOrderService(string baseurl)
        {
            BaseUrl = baseurl;
        }


        /// <summary>
        /// 创建消息订单
        /// </summary>
        /// <param name="body">body</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
#if NET40 || NET45
        public string CreateMessagerOrder(CreateOrderCommand body)
#else
        public async Task<string> CreateMessagerOrderAsync(CreateOrderCommand body)
#endif
        {
            var client = new RestClient(BaseUrl);
            var resource = @"api/v1/orders";
#if NET40 || NET45
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(body);
            var responseData = client.Execute(request);
#else
            var request = new RestRequest(resource, Method.Post);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(body);
            var responseData = await client.ExecuteAsync(request);
#endif
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                return "OK";
            }
            else
            {
                ErrorData responseFail = null;
                try
                {
                    responseFail = JsonConvert.DeserializeObject<ErrorData>(responseData.Content);
                }
                catch (Exception)
                {
                    //ignore
                }
                if (responseFail == null)
                {
                    var message = "MessageCore:StatusCode:" + responseData.StatusCode + ",Content:" + responseData.Content;
                    throw new Exception(message);
                }
                var messageFail = "MessageCore:TicketId:" + responseFail.TicketId + ",ErrorMessage:" + string.Join(",", responseFail?.Errors?.Select(item => item.Code + ":" + item.Message));
                throw new Exception(messageFail);
            }
        }

        /// <summary>
        /// 创建简单邮件消息订单
        /// </summary>
        /// <param name="body">body</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
#if NET40 || NET45
        public string CreateSimpleEmailOrder(CreateSimpleEmailCommand body)
#else
        public async Task<string> CreateSimpleEmailOrder(CreateSimpleEmailCommand body)
#endif
        {
            var client = new RestClient(BaseUrl);
            var resource = @"api/v1/orders/SimpleEmail";
#if NET40 || NET45
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(body);
            var responseData = client.Execute(request);
#else
            var request = new RestRequest(resource, Method.Post);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(body);
            var responseData = await client.ExecuteAsync(request);
#endif
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                return "OK";
            }
            else
            {
                ErrorData responseFail = null;
                try
                {
                    responseFail = JsonConvert.DeserializeObject<ErrorData>(responseData.Content);
                }
                catch (Exception)
                {
                    //ignore
                }
                if (responseFail == null)
                {
                    var message = "MessageCore:StatusCode:" + responseData.StatusCode + ",Content:" + responseData.Content;
                    throw new Exception(message);
                }
                var messageFail = "MessageCore:TicketId:" + responseFail.TicketId + ",ErrorMessage:" + string.Join(",", responseFail?.Errors?.Select(item => item.Code + ":" + item.Message));
                throw new Exception(messageFail);
            }
        }

        /// <summary>
        /// 创建简单企业微信消息订单
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
#if NET40 || NET45
        public string CreateSimpleEnterpriseWeChatOrder(CreateSimpleEnterpriseWeChatCommand body)
#else
        public async Task<string> CreateSimpleEnterpriseWeChatOrder(CreateSimpleEnterpriseWeChatCommand body)
#endif
        {
            var client = new RestClient(BaseUrl);
            var resource = @"api/v1/orders/SimpleEnterpriseWeChat";
#if NET40 || NET45
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(body);
            var responseData = client.Execute(request);
#else
            var request = new RestRequest(resource, Method.Post);
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(body);
            var responseData = await client.ExecuteAsync(request);
#endif
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                return "OK";
            }
            else
            {
                ErrorData responseFail = null;
                try
                {
                    responseFail = JsonConvert.DeserializeObject<ErrorData>(responseData.Content);
                }
                catch (Exception)
                {
                    //ignore
                }
                if (responseFail == null)
                {
                    var message = "MessageCore:StatusCode:" + responseData.StatusCode + ",Content:" + responseData.Content;
                    throw new Exception(message);
                }
                var messageFail = "MessageCore:TicketId:" + responseFail.TicketId + ",ErrorMessage:" + string.Join(",", responseFail?.Errors?.Select(item => item.Code + ":" + item.Message));
                throw new Exception(messageFail);
            }
        }
    }
}
