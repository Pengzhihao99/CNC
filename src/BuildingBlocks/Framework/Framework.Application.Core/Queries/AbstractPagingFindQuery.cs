using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Core.Queries
{
    /// <summary>
    /// 表示分页查找请求
    /// </summary>

    public abstract class AbstractPagingFindQuery
    {
        #region 属性
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否分页，为true则分页，为false则不分页。默认分页
        /// </summary>
        public bool Paging { get; set; }

        /// <summary>
        /// 排序字段列表
        /// </summary>
        public List<OrderField> ViewOrderFields { get; set; }

        #endregion

        #region 初始化
        public AbstractPagingFindQuery()
        {
            PageNumber = 1;
            PageSize = 20;
            Paging = true;
            ViewOrderFields = new List<OrderField>();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置升序
        /// </summary>
        public void SetAscendOrder(string field)
        {
            SetOrder(OrderFlag.Ascending, field);
        }
        /// <summary>
        /// 设置降序
        /// </summary>
        public void SetDescendOrder(string field)
        {
            SetOrder(OrderFlag.Descending, field);
        }
        private void SetOrder(OrderFlag flag, string field)
        {
            ViewOrderFields.Add(new OrderField() { Flag = flag, Field = field });
        }
        #endregion
    }



    /// <summary>
    /// 排序标志
    /// </summary>
    public enum OrderFlag
    {
        //
        // 摘要:
        //     升序
        Ascending = 0,
        //
        // 摘要:
        //     降序
        Descending = 1
    }

    /// <summary>
    /// 排序字段信息
    /// </summary>
    public class OrderField
    {
        public OrderFlag Flag { get; set; }
        public string Field { get; set; }
    }
}
