using AutoMapper;
using Framework.Infrastructure.Crosscutting.Adapter;

namespace Framework.Infrastructure.Adapter.AutoMapper
{
    /// <summary>
    /// 基于AutoMapper实现的类型转换
    /// </summary>
    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        private readonly IMapper _mapper;

        public AutoMapperTypeAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 一个类型为  <typeparamref name="TSource"/> 的对象，适配为一个类型为 <typeparamref name="TTarget"/> 的对象。
        /// </summary>
        /// <typeparam name="TSource">需要被适配的对象的类型</typeparam>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="source">需要被适配的对象</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TTarget : class
            where TSource : class
        {
            return _mapper.Map<TSource, TTarget>(source);
        }

        /// <summary>
        /// 一个类型为  <typeparamref name="TSource"/> 的对象，适配为一个类型为 <typeparamref name="TTarget"/> 的对象列表。
        /// </summary>
        /// <typeparam name="TSource">需要被适配的对象的类型</typeparam>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="sources">需要被适配的对象</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        public IList<TTarget> Adapt<TSource, TTarget>(IEnumerable<TSource> sources)
            where TTarget : class
            where TSource : class
        {
            return _mapper.Map<IEnumerable<TSource>, IList<TTarget>>(sources);
        }

        /// <summary>
        /// 将对象适配为一个类型为 <typeparamref name="TTarget"/> 的对象。
        /// </summary>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="source">需要被适配的对象</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        public TTarget Adapt<TTarget>(object source) where TTarget : class
        {
            return _mapper.Map<TTarget>(source);
        }

        /// <summary>
        /// 将对象集合适配为一个类型为 <typeparamref name="TTarget"/> 的对象列表。
        /// </summary>
        /// <typeparam name="TTarget">目标对象的类型</typeparam>
        /// <param name="sources">需要被适配的对象列表</param>
        /// <returns>目标类型为 <typeparamref name="TTarget"/> 的对象</returns>
        public IList<TTarget> AdaptList<TTarget>(IEnumerable<object> sources) where TTarget : class
        {
            return _mapper.Map<IEnumerable<object>, IList<TTarget>>(sources);
        }
    }
}