using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Helpers
{
    public class GeneralHelper
    {
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public static DataTable CreateDataTable(IEnumerable source)
        {
            var table = new DataTable();
            int index = 0;
            var properties = new List<PropertyInfo>();
            foreach (var obj in source)
            {
                if (index == 0)
                {
                    foreach (var property in obj.GetType().GetProperties())
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            continue;
                        }
                        properties.Add(property);
                        table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                    }
                }
                object[] values = new object[properties.Count];
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(obj);
                }
                table.Rows.Add(values);
                index++;
            }
            return table;
        }
        public static void SetPropertyValue(object obj, string name, object value)
        {
            Type type = obj.GetType();
            type.GetProperty(name).SetValue(obj, value);
        }

        private static MethodInfo baseLoop(Type type, object obj, Func<MethodInfo, bool> condition, object[] prms)
        {
            MethodInfo method = null;

            do
            {
                method = type.GetMethods().Where(f => condition(f)).FirstOrDefault();
                if (method != null) return method;
                var rtn = interfaceLoop(type, obj, condition, prms);
                if (rtn != null) return rtn;
            } while (type != null);

            return null;
        }
        private static MethodInfo interfaceLoop(Type type, object obj, Func<MethodInfo, bool> condition, object[] prms)
        {
            MethodInfo method = null;
            Type[] inferfaces = type.GetInterfaces();

            foreach (var each in inferfaces)
            {
                method = each.GetMethods().Where(f => condition(f)).FirstOrDefault();
                if (method != null) return method;
                var rtn = interfaceLoop(each, obj, condition, prms);
                if (rtn != null) return rtn;
                rtn = baseLoop(each, obj, condition, prms);
                if (rtn != null) return rtn;
            }

            return null;
        }
        public static MethodInfo GetFunction(object obj, string functionname)
        {
            return GetFunction(obj, functionname, null as object[]);
        }

        public static MethodInfo GetFunction(object obj, string functionname, params object[] prms)
        {
            Func<MethodInfo, bool> condition = (MethodInfo a) =>
            {
                if (a.Name != functionname) return false;

                var innerPrms = a.GetParameters();

                if (prms == null) return innerPrms == null || innerPrms.Length == 0;

                if (innerPrms.Length != prms.Length) return false;

                for (int i = 0; i < prms.Length; ++i)
                {
                    if (prms[i] == null) continue;
                    if (innerPrms[i].ParameterType != typeof(object) && innerPrms[i].ParameterType != prms[i].GetType())
                    {
                        return false;
                    }
                }

                return true;
            };

            Type type = obj.GetType();
            var rtn = baseLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            rtn = interfaceLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            throw new Exception("No valid method found");
        }


        public static MethodInfo GetFunction(object obj, string functionname, params Type[] prms)
        {
            Func<MethodInfo, bool> condition = (MethodInfo a) =>
            {
                if (a.Name != functionname) return false;

                var innerPrms = a.GetParameters();

                if (prms == null) return innerPrms == null || innerPrms.Length == 0;

                if (innerPrms.Length != prms.Length) return false;

                for (int i = 0; i < prms.Length; ++i)
                {
                    if (prms[i] == null) continue;
                    if (innerPrms[i].ParameterType != typeof(object) && innerPrms[i].ParameterType != prms[i])
                    {
                        return false;
                    }
                }

                return true;
            };

            Type type = obj.GetType();
            var rtn = baseLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            rtn = interfaceLoop(type, obj, condition, prms);

            if (rtn != null) return rtn;

            throw new Exception("No valid method found");
        }
    }
}
