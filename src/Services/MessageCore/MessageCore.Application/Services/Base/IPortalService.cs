using Framework.Infrastructure.Crosscutting;
using MessageCore.Application.DataTransferModels.Portal;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services.Base
{
    public interface IPortalService
    {
        string Login(string userName, string password);


        void Logout(string token);


        void ValidateToken(string token);


        UserInfo GetUserInfo(string token);

    }
}
