using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duplex.Infrastructure
{
    // extention methods to help with reflection tasks
    internal static class ReflectionExtensions
    {
        // get an attribute
        public static T GetAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>().First();
        }

        // get multiple attributes
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>();
        }

        // check to see if attribute exists
        public static bool HasAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).Any();
        }

        // will return null if attribute does not exist else will retreive the attribute
        public static T GetAttributeOrNull<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
        }

        // gets all the methods with the specified attribute
        public static MethodInfo[] GetMethodsWithAttribute<T>(this Type t) where T : Attribute
        {
            return t.GetMethods().Where(x => x.HasAttribute<T>()).ToArray();
        }


    }
}
