namespace MessageCore.AdminApi.Model
{
    public class RolePageParams
    {
        public int? page { get; set; }
        public int? pageSize { get; set; }
        //public string? roleName { get; set; }
        //public string? status { get; set; }
    }

    public class BasicFetchResult
    {
        public RoleListItem[] items { get; set; }
        public int? total { get; set; }
    }

    //public class RoleParams
    //{
    //    public string? roleName { get; set; }
    //    public string? status { get; set; }
    //}

    public class RoleListItem
    {
        public string? id { get; set; }
        public string? senderName { get; set; }
        public string? token { get; set; }
        public bool status { get; set; }
        public string? updateOn { get; set; }
        public string? remark { get; set; }
    }

    public class Result<T>
    {
        public int? code { get; set; }
        public string? type { get; set; }
        public string? Message { get; set; }
        public T? result { get; set; }
    }

    public class LoginParams
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RoleInfo
    {
        public string RoleName { get; set; }
        public string Value { get; set; }
    }

    public class LoginResultModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public RoleInfo Role { get; set; }
    }

    public class GetUserInfoModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string Avatar { get; set; }
        public string Desc { get; set; }
        public RoleInfo[] Roles { get; set; }
    }
}
