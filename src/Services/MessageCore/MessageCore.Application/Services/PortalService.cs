using Framework.Infrastructure.Crosscutting;
using MessageCore.Application.DataTransferModels.Portal;
using MessageCore.Application.Services.Base;
using MessageCore.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using Scriban.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services
{
    public class PortalService : IPortalService
    {
        private readonly IConfiguration _configuration;
        public PortalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Login(string userName, string password)
        {
            Check.Argument.IsNotNullOrEmpty(userName, nameof(userName));
            Check.Argument.IsNotNullOrEmpty(password, nameof(password));

            var client = new RestClient(_configuration.GetValue<string>("Portal:url"));
            var request = new RestRequest("/api/v1/login", Method.Post);
            request.AddParameter("identity", userName, ParameterType.QueryString);
            request.AddParameter("password", password, ParameterType.QueryString);
            request.AddParameter("source", "appClient", ParameterType.QueryString);

            var reponseDate = client.Execute(request);
            LoginResponse response = null;

            try
            {
                response = JsonConvert.DeserializeObject<LoginResponse>(reponseDate.Content);
            }
            catch (Exception ex)
            {

            }
            if (response == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.LoginFail, string.Format(ErrorMessage.LoginFail, userName, reponseDate.Content));
            }
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.LoginFail, string.Format(ErrorMessage.LoginFail, userName, response.Message));
            }

            if (!response.IsEmployee)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.LoginFail, string.Format(ErrorMessage.LoginFail, userName,"Login Fail!"));
            }

            return response.Token;

        }
        public void Logout(string token)
        {
            Check.Argument.IsNotNullOrEmpty(token, nameof(token));
            var client = new RestClient(_configuration.GetValue<string>("Portal:url"));
            var request = new RestRequest("/api/v1/logout/" + token, Method.Post);

            var reponseDate = client.Execute(request);

            LogoutResponse response = null;

            try
            {
                response = JsonConvert.DeserializeObject<LogoutResponse>(reponseDate.Content);
            }
            catch (Exception ex)
            {

            }
            if (response == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.LogoutFail, string.Format(ErrorMessage.LogoutFail, reponseDate.Content));
            }
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.LogoutFail, string.Format(ErrorMessage.LogoutFail, response.Message));
            }
        }
        public void ValidateToken(string token)
        {
            Check.Argument.IsNotNullOrEmpty(token, nameof(token));
            var client = new RestClient(_configuration.GetValue<string>("Portal:url"));
            var request = new RestRequest("/api/v1/token/" + token, Method.Post);

            var reponseDate = client.Execute(request);

            LogoutResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<LogoutResponse>(reponseDate.Content);
            }
            catch (Exception ex)
            {

            }
            if (response == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.ValidateTokenError, string.Format(ErrorMessage.ValidateTokenError, reponseDate.Content));
            }
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.ValidateTokenError, string.Format(ErrorMessage.ValidateTokenError, response.Message));
            }
        }
        public UserInfo GetUserInfo(string token)
        {
            Check.Argument.IsNotNullOrEmpty(token, nameof(token));
            var client = new RestClient(_configuration.GetValue<string>("Portal:url"));
            var request = new RestRequest("/api/users/token/" + token, Method.Get);
            var reponseDate = client.Execute(request);

            UserInfo response = null;

            try
            {
                response = JsonConvert.DeserializeObject<UserInfo>(reponseDate.Content);
            }
            catch (Exception ex)
            {

            }
            if (response == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.GetUserInfoError, string.Format(ErrorMessage.GetUserInfoError, reponseDate.Content));
            }
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.GetUserInfoError, string.Format(ErrorMessage.GetUserInfoError, response.Message));
            }
            return response;
        }
    }
}
