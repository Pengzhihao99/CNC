using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    /// <summary>
    /// 表示一个单页数据分页的信息的集合类型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> : IEnumerable<T>, ICollection<T>
    {
        /// <summary>
        /// 初始化一个新的<c>SinglePageResult{T}</c>类型的实例。
        /// </summary>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="data">当前页面的数据。</param>
        public PagedResult(int pageNumber, int pageSize, List<T> data)
        {
            this.PageSize = pageSize;
            this.Data = data;
            PageNumber = pageNumber;
        }

        /// <summary>
        /// 获取或设置页码。
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// 获取或设置页面数量大小。
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 获取或设置当前页面的数据。
        /// </summary>
        public IList<T> Data { get; private set; }

        #region IEnumerable/IEnumerable<T> Members

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的 IEnumerator 对象。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。 （继承自 IEnumerable。）
        /// </summary>
        /// <returns>一个可用于循环访问集合的 IEnumerator 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// 将某项添加到 ICollection{T} 中。
        /// </summary>
        /// <param name="item">要添加到 ICollection{T} 的对象。</param>
        public void Add(T item)
        {
            this.Data.Add(item);
        }

        /// <summary>
        /// 从 ICollection{T} 中移除所有项。
        /// </summary>
        public void Clear()
        {
            this.Data.Clear();
        }

        /// <summary>
        /// 确定 ICollection{T} 是否包含特定值。
        /// </summary>
        /// <param name="item">要在 ICollection{T} 中定位的对象。</param>
        /// <returns>如果在 ICollection{T} 中找到 item，则为 true；否则为 false。</returns>
        public bool Contains(T item)
        {
            return this.Data.Contains(item);
        }

        /// <summary>
        /// 从特定的 Array 索引开始，将 ICollection{T} 的元素复制到一个 Array 中。
        /// </summary>
        /// <param name="array">作为从 ICollection{T} 复制的元素的目标的一维 Array。 Array 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex">array 中从零开始的索引，从此索引处开始进行复制。</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Data.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取 ICollection{T} 中包含的元素数。
        /// </summary>
        public int Count
        {
            get { return this.Data.Count; }
        }

        /// <summary>
        /// 获取一个值，该值指示 ICollection{T} 是否为只读。
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 从 ICollection{T} 中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 ICollection{T} 中移除的对象。</param>
        /// <returns>如果已从 ICollection{T} 中成功移除 item，则为 true；否则为 false。 如果在原始 ICollection{T} 中没有找到 item，该方法也会返回 false。 </returns>
        public bool Remove(T item)
        {
            return this.Data.Remove(item);
        }

        #endregion
    }
}
