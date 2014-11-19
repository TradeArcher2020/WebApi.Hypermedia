using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Hypermedia
{
    public static class ObjectExtensions
    {
        public static TCastResultType NullSafeGetValue<TSource, TResult, TCastResultType>(this TSource source, Expression<Func<TSource, TResult>> expression, TCastResultType defaultValue, Func<object, TCastResultType> convertToResultToAction)
        {
            var value = GetValue(expression, source);
            return value == null ? defaultValue : convertToResultToAction.Invoke(value);
        }

        public static TResult NullSafeGetValue<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> expression, TResult defaultValue)
        {
            var value = GetValue(expression, source);
            return value == null ? defaultValue : (TResult)value;
        }

        public static TCastResultType NullSafeGetValue<TSource, TCastResultType>(this TSource source, string fullPropertyPathName, TCastResultType defaultValue, Func<object, TCastResultType> convertToResultToAction)
        {
            var value = GetValue(fullPropertyPathName, source);
            return value == null ? defaultValue : convertToResultToAction.Invoke(value);
        }

        public static TResult NullSafeGetValue<TSource, TResult>(this TSource source, string fullPropertyPathName, TResult defaultValue)
        {
            var value = GetValue(fullPropertyPathName, source);
            return value == null ? defaultValue : (TResult)value;
        }

        public static string GetFullPropertyPathName<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            MemberExpression memberExp;
            if (!TryFindMemberExpression(exp.Body, out memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow
        private static bool TryFindMemberExpression
        (Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
            );
        }

        private static object GetValue<TSource, TResult>(Expression<Func<TSource, TResult>> expression, TSource source)
        {
            string fullPropertyPathName = GetFullPropertyPathName(expression);
            return GetNestedPropertyValue(fullPropertyPathName, source);
        }

        private static object GetValue<TSource>(string fullPropertyPathName, TSource source)
        {
            return GetNestedPropertyValue(fullPropertyPathName, source);
        }

        private static TResult GetValue<TSource, TResult>(string fullPropertyPathName, TSource source)
        {
            return (TResult)GetNestedPropertyValue(fullPropertyPathName, source);
        }

        private static object GetNestedPropertyValue(string name, object obj)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return obj;
            }
            PropertyInfo info;
            foreach (var part in name.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }
                var type = obj.GetType();
                if (obj is IEnumerable)
                {
                    type = (obj as IEnumerable).GetType();
                    var methodInfo = type.GetMethod("get_Item");
                    var index = int.Parse(part.Split('(')[1].Replace(")", string.Empty));
                    try
                    {
                        obj = methodInfo.Invoke(obj, new object[] { index });
                    }
                    catch (Exception)
                    {
                        obj = null;
                    }
                }
                else
                {
                    info = type.GetProperty(part);
                    if (info == null)
                    {
                        return null;
                    }
                    obj = info.GetValue(obj, null);
                }
            }
            return obj;
        }
    }
}
