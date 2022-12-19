using EzComs.Common.CustomExceptions;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EzComs.Model.ConversionContext
{
    public class JsonAdapter
    {
        public JsonAdapter() { }

        public static string Convert(string source, string from, string to) {
            
           
            dynamic deserializedJsonString = JsonConvert.DeserializeObject<dynamic>(from);
            if (deserializedJsonString == null) { throw new ConversionException<string, dynamic>(); }

            Dictionary<string, string> valuePaths = GetPathsFromObject(deserializedJsonString);
            Dictionary<string, object> pathsWithValue = new Dictionary<string, object>();

        }


        /// <summary>
        /// Takes a jsonstring that is converted to a dynamic object and checks where the value is a variable described by giving it a ..value..
        /// </summary>
        /// <param name="deserializedJsonString">The deserialized string that contains all known or possible paths</param>
        /// <returns>A Dictionary<string, string> with all variable names and paths</returns>
        public static Dictionary<string, string> GetPathsFromObject(dynamic deserializedJsonString, string path = "", Dictionary<string, string> paths = new())
        {
            foreach (PropertyInfo prop in deserializedJsonString.GetType().GetProperties())
            {
                paths = GetPath(deserializedJsonString, prop, path, paths);
            }
            return paths;
        }
        private static Dictionary<string, string> GetPath(dynamic deserializedJsonString, PropertyInfo prop, string path, Dictionary<string, string> paths)
        {
            var propvalue = prop.GetValue(deserializedJsonString, null);
            if (propvalue is string)
            {
                if ((propvalue as string).Contains(".."))
                {
                    path = AppendPath(prop.Name, path);
                    paths.Add(propvalue as string, path);
                }
            }
            else if (propvalue is Array)
            {
                var propArray = propvalue as Array;
                path = AppendPath(prop.Name, path);
                GetPathsFromArray(propArray, path, paths);
                
            } else if(propvalue is not null && Convert.GetTypeCode(propvalue) != TypeCode.Object)
            {
                path = AppendPath(prop.Name, path);
                paths = GetPathsFromObject((dynamic)propvalue, path, paths);
            }

            return paths;
        }

        private static Dictionary<string, string> GetPathsFromArray(Array array, string path, Dictionary<string, string> paths)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var value = array.GetValue(i);
                if (value is string)
                {
                    if ((propvalue as string).Contains(".."))
                    {
                        path +=$"[{i}]";
                        paths.Add(propvalue as string, path);
                    }
                } else if (value is Array)
                {
                    path += $"[{i}]";
                    GetPathsFromArray(value as Array, path, paths);
                } else if (value is not null && Convert.GetTypeCode(value) != TypeCode.Object)
                {
                    path += $"[{i}]";
                    GetPathsFromObject(value as dynamic, path, paths);
                }
            }
        }

        private static string AppendPath(string value, string path = "")
        {
            if(path != "") {  path+= ".";}
            return path += value;
        }

        /// <summary>
        /// Gets the value of a dynamic object from a value info path
        /// </summary>
        /// <param name="source">The dynamic source of which to get the value from based on the path info</param>
        /// <param name="valueInfo">The value info object that </param>
        /// <returns></returns>
        private ValueInfo GetPathValue(dynamic source, ValueInfo valueInfo)
        {
            string path = valueInfo.Path;
            dynamic currentSource = source;
           

            while (path.Contains("."))
            {
                //go into the first value
                currentSource = currentSource[path.Split(".")[0]];
                
                path = path.Replace(path.Split(".")[0] + ".", "");
            }

            if (!valueInfo.IsListItem)
            {
                valueInfo.Value = currentSource[path];
            } else
            {
                int index = int.Parse(path.Split("[")[1].Replace("]", ""));
                path = path.Split("[")[0];
                valueInfo.Value = (currentSource[path] as Array).GetValue(index);
            }
            return valueInfo;
        }

        private class ValueInfo
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public bool IsListItem { get; set; }
            public object Value { get; set; }
        }

       
    }
}
