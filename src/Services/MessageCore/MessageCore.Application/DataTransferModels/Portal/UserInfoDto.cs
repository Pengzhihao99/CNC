using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.DataTransferModels.Portal
{
    public class UserInfoDto
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 内部员工工号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 用户角色码列表
        /// </summary>
        public IEnumerable<string> RoleCodes { get; set; }

        /// <summary>
        /// 用户权限码列表
        /// </summary>
        public IEnumerable<string> PermissionCodes { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }
    }
}
