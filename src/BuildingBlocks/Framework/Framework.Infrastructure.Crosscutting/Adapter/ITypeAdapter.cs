using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Crosscutting.Adapter
{
    /// <summary>
    /// 类型转换器，实现类型转换，如Domain为DTO.
    /// 一般通过一些自动转换工具或者类库 ( AutoMapper, EmitMapper, ValueInjecter...)，配置好类型转换契约，自动适配且转换为目标类型。
    /// </summary>
    public interface ITypeAdapter
    {
        /// <summary>
        /// 一个类型为  <typeparamref name="TSource"/> 的对象集合，适配为一个类型为 <typeparamref name="TTarget"/> 的对象。
        /// </summary>
        /// <typeparam name="TSource">需要被适配的对象的类型</typeparam>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="source">需要被适配的对象</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        TTarget Adapt<TSource, TTarget>(TSource source)
            where TTarget : class
            where TSource : class;

        /// <summary>
        /// 一个类型为  <typeparamref name="TSource"/> 的对象，适配为一个类型为 <typeparamref name="TTarget"/> 的对象列表。
        /// </summary>
        /// <typeparam name="TSource">需要被适配的对象的类型</typeparam>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="sources">需要被适配的对象</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        IList<TTarget> Adapt<TSource, TTarget>(IEnumerable<TSource> sources)
            where TTarget : class
            where TSource : class;

        /// <summary>
        /// 将对象适配为一个类型为 <typeparamref name="TTarget"/> 的对象。
        /// </summary>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="source">需要被适配的对象列表</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        TTarget Adapt<TTarget>(object source) where TTarget : class;

        /// <summary>
        /// 将对象集合适配为一个类型为 <typeparamref name="TTarget"/> 的对象列表。
        /// </summary>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="sources">需要被适配的对象列表</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        IList<TTarget> AdaptList<TTarget>(IEnumerable<object> sources) where TTarget : class;
    }
}
