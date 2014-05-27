using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace engine.util
{
    public static class ReflectionUtils
    {
        #region Data

        public static bool IsDefaultValue(Type t, object value)
        {
            return value.Equals(t.IsValueType ? Activator.CreateInstance(t) : null);
        }

        public static bool IsDefaultValue<T>(T value)
        {
            return value.Equals(default(T));
        }

        #endregion

        #region Inheritance

        public static IEnumerable<Type> GetSubclasses<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof (T))).Distinct();
        }

        #endregion

        #region Cloning

        public static T Clone<T>(T instance)
        {
            if (typeof (T).IsValueType)
                return instance;

            var clone = (T) Activator.CreateInstance(typeof (T));

            throw new NotImplementedException();
        }

        public static void Merge(Type t, object master, object slave, bool mergeFields = true, bool mergeProperties = true)
        {
            if (mergeFields)
                foreach (var f in GetFields(t))
                {
                    if (IsDefaultValue(f.GetValue(master)))
                        f.SetValue(master, f.GetValue(slave));
                    if (!f.FieldType.IsValueType)
                        Merge(f.GetValue(master), f.GetValue(slave), mergeFields, mergeProperties);
                }
            if (mergeProperties)
                foreach (var p in GetProperties(t))
                {
                    if (IsDefaultValue(p.GetValue(master)))
                        p.SetValue(master, p.GetValue(slave));
                    if (!p.PropertyType.IsValueType)
                        Merge(p.GetValue(master), p.GetValue(slave), mergeFields, mergeProperties);
                }
        }

        public static void Merge<T>(T master, T slave, bool mergeFields = true, bool mergeProperties = true)
        {
            Merge(typeof(T), master, slave, mergeFields, mergeProperties);
        }

        #endregion

        #region Fields

        public static IEnumerable<FieldInfo> GetFields(Type t)
        {
            return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static IEnumerable<FieldInfo> GetFields<T>()
        {
            return GetFields(typeof (T));
        }

        public static IEnumerable<FieldInfo> GetUnassignedFields<T>(T instance)
        {
            return GetFields<T>().Where(f => IsDefaultValue(f.FieldType, f.GetValue(instance)));
        }

        #endregion

        #region Properties

        public static IEnumerable<PropertyInfo> GetProperties(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return GetProperties(typeof (T));
        }

        public static IEnumerable<PropertyInfo> GetUnassignedProperties<T>(T instance)
        {
            return GetProperties<T>().Where(f => IsDefaultValue(f.PropertyType, f.GetValue(instance)));
        }

        #endregion

    }
}
