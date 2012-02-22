using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duplex.Infrastructure
{
    internal static class ReflectionExtensions
    {
        public static T GetAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>().First();
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>();
        }

        public static bool HasAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).Any();
        }

        public static T GetAttributeOrNull<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
        }

        public static MethodInfo[] GetMethodsWithAttribute<T>(this Type t) where T : Attribute
        {
            return t.GetMethods().Where(x => x.HasAttribute<T>()).ToArray();
        }


    }
}
