using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DouyinTest.Utils
{
    public static class TransformExtension
    {
        static TransformExtension()
        {
            
            Mapper.Initialize(p =>
            {
                p.CreateMissingTypeMaps = true;
            });
        }

        /// <summary>
        /// 将此对象转换为 <typeparamref name="TDestination"/> 的对象。
        /// </summary>
        /// <typeparam name="TDestination">目标类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="onAfter">需要在转换完成后执行的工作。</param>
        /// <param name="onBefore">需要在转换开始之前执行的工作。</param>
        /// <returns></returns>
        public static TDestination TransformTo<TDestination>(this object source, Action<dynamic, TDestination> onAfter = null, Action<dynamic, TDestination> onBefore = null)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            var options = CreateOptions(onAfter, onBefore);

            return source.TransformToInternal(options);
        }

        /// <summary>
        /// 将 <typeparamref name="TSource"/> 的对象转换为 <typeparamref name="TDestination"/> 的对象。
        /// </summary>
        /// <typeparam name="TDestination">目标类型。</typeparam>
        /// <typeparam name="TSource">原类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="onAfter">需要在转换完成后执行的工作。</param>
        /// <param name="onBefore">需要在转换开始之前执行的工作。</param>
        /// <returns></returns>
        public static TDestination TransformTo<TDestination, TSource>(this TSource source, Action<TSource, TDestination> onAfter = null, Action<TSource, TDestination> onBefore = null)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            var options = CreateOptions(onAfter, onBefore);

            return source.TransformToInternal(options);
        }

        /// <summary>
        /// 将此列表转换为 <typeparamref name="TDestination"/> 的列表。
        /// </summary>
        /// <typeparam name="TDestination">目标类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="onAfter">需要在转换完成后执行的工作。</param>
        /// <param name="onBefore">需要在转换开始之前执行的工作。</param>
        /// <returns></returns>
        public static IEnumerable<TDestination> TransformTo<TDestination>(this IEnumerable source, Action<dynamic, TDestination> onAfter = null, Action<dynamic, TDestination> onBefore = null)
        {
            var options = CreateOptions(onAfter, onBefore);

            foreach (var item in source)
            {
                yield return item.TransformToInternal(options);
            }
        }

        /// <summary>
        /// 将 <typeparamref name="TSource"/> 的列表转换为 <typeparamref name="TDestination"/> 的列表。
        /// </summary>
        /// <typeparam name="TDestination">目标类型。</typeparam>
        /// <typeparam name="TSource">原类型。</typeparam>
        /// <param name="source"></param>
        /// <param name="onAfter">需要在转换完成后执行的工作。</param>
        /// <param name="onBefore">需要在转换开始之前执行的工作。</param>
        /// <returns></returns>
        public static IEnumerable<TDestination> TransformTo<TDestination, TSource>(this IEnumerable<TSource> source, Action<TSource, TDestination> onAfter = null, Action<TSource, TDestination> onBefore = null)
        {
            var options = CreateOptions(onAfter, onBefore);

            foreach (var item in source)
            {
                yield return item.TransformToInternal(options);
            }
        }

        private static TDestination TransformToInternal<TDestination, TSource>(this TSource source, Action<IMappingOperationOptions<TSource, TDestination>> options)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return Mapper.Map(source, options);
        }

        private static Action<IMappingOperationOptions<TSource, TDestination>> CreateOptions<TSource, TDestination>(Action<TSource, TDestination> onAfter = null, Action<TSource, TDestination> onBefore = null)
        {
            Action<IMappingOperationOptions<TSource, TDestination>> options = new Action<IMappingOperationOptions<TSource, TDestination>>(p =>
            {
                if (onAfter != null)
                {
                    p.AfterMap(onAfter);
                }

                if (onBefore != null)
                {
                    p.BeforeMap(onBefore);
                }
            });

            return options;
        }

        /// <summary>
        /// 将 <typeparamref name="TSource"/> 的值合并给 <typeparamref name="TDestination"/>。
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <param name="ignoredProperties">要忽略的属性。</param>
        /// <param name="ignoreNull">当合并源对象为 <c>null</c> 时如何处理? 默认为 <c>false</c>。</param>
        /// <param name="onAfter">需要在合并完成后执行的工作。</param>
        /// <param name="onBefore">需要在合并开始之前执行的工作。</param>
        /// <returns></returns>
        public static TDestination MergeFrom<TDestination, TSource>(this TDestination dest, TSource source, string[] ignoredProperties = null, bool ignoreNull = false, Action<TDestination, TSource> onAfter = null, Action<TDestination, TSource> onBefore = null)
        {
            if (dest == null)
            {
                throw new ArgumentNullException(nameof(dest));
            }

            if (onBefore != null)
            {
                onBefore(dest, source);
            }

            if (source != null)
            {
                Dictionary<string, PropertyInfo> entityProperties = typeof(TDestination).GetProperties().ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

                foreach (var item in typeof(TSource).GetProperties())
                {
                    if (ignoredProperties?.Contains(item.Name, StringComparer.OrdinalIgnoreCase) == true)
                    {
                        continue;
                    }

                    if (!entityProperties.ContainsKey(item.Name))
                    {
                        continue;
                    }

                    var readOnly = item.GetCustomAttribute<ReadOnlyAttribute>();

                    if (readOnly?.IsReadOnly == true)
                    {
                        continue;
                    }

                    object value = item.GetValue(source);

                    if (value == null && ignoreNull)
                    {
                        continue;
                    }

                    if (value is Enum)
                    {
                        if (entityProperties[item.Name].PropertyType == typeof(int))
                        {
                            entityProperties[item.Name].SetValue(dest, (int)value);
                        }
                        else
                        {
                            entityProperties[item.Name].SetValue(dest, value.ToString());
                        }
                    }
                    else
                    {
                        entityProperties[item.Name].SetValue(dest, value);
                    }
                }
            }

            if (onAfter != null)
            {
                onAfter(dest, source);
            }

            return dest;
        }
    }
}
