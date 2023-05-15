using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.DataTransferModels.Portal
{

    public class LoginResponse
    {
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsApi { get; set; }
        public string Token { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

    public class LogoutResponse
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }


    public class UserInfo
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string EmployeeNo { get; set; }
        public string ClientNo { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Desc { get; set; }
        public bool IsApproved { get; set; }
        public string UserType { get; set; }
        public string[] RoleCodes { get; set; }
        public string[] PermissionCodes { get; set; }
        public bool HasError { get; set; }

        public string Message { get; set; }
    }

}
