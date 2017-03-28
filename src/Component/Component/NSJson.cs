using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class NSJson
    {
        public static bool Parser(String json, IDictionary dict)
        {
            //IDictionary dict = new Dictionary<string, Object>();
            //json = json.Replace("\r\n", "");
            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(json);
                JsonToDirect(jObject, dict);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                return false;
            }

            return true;
        }

        public static void Enum(IDictionary parent_dict)
        {
            foreach (String key in parent_dict.Keys)
            {
                Object value = parent_dict[key];

                if (value is String)
                {
                    Debug.WriteLine(key + " : " + value.ToString());
                }
                else if (value is IDictionary)
                {
                    Debug.WriteLine(key);
                    Enum((IDictionary)value);
                }
                else if (value is IList)
                {
                    Debug.WriteLine(key);
                    Enum((IList)value);
                }
                
            }
        }

        public static void Enum(IList parent_list)
        {
            foreach (Object obj in parent_list)
            {
                if (obj is IDictionary)
                {
                    Enum((IDictionary)obj);
                }
                else if (obj is IList)
                {
                    Enum((IList)obj);
                }
                
            }
        }

        public static bool IsExist(IDictionary parent_dict, params String[] keys)
        {
            //bool b = IsExist(dict, "Audio", "Realtek Audio Driver", "6-0-1-7997", "HDAUDIO\\\\FUNC_01&VEN_10EC&DEV_", "1", "2");
            //Console.WriteLine("Is exist = {0}", b);
            
            IDictionary dict = parent_dict;
            String str = "";

            foreach (String key in keys)
            {
                if (str == null)
                {
                    return false;
                }
                else if (str.Length > 0)
                {
                    str = null;
                    if (str != key)
                    {
                        return false;
                    }
                }
                else if (dict.Contains(key))
                {
                    if (dict[key] is IDictionary)
                    {
                        dict = (IDictionary)dict[key];
                        
                    }
                    else
                    {
                        str = dict[key].ToString();
                    }

                }
                else
                {
                    return false;
                }
            }


            return true;
        }

        public static Object GetData(IDictionary parent_dict, params String[] keys)
        {
            /*
             EX
            var obj = GetData(dict, "Audio", "Realtek Audio Driver", "6-0-1-7997");
            if (obj != null)
            {
                if (obj.GetType() == typeof(String))
                {
                    Console.WriteLine(obj.ToString());
                }
                else
                {
                    EnumDirect((IDictionary)obj);
                }
            }
           */


            IDictionary dict = parent_dict;
            String str = "";

            foreach (String key in keys)
            {
                if (str.Length > 0)
                {
                    return null;
                }
                else if (dict.Contains(key))
                {
                    if (dict[key].GetType() == typeof(String))
                    {
                        str = dict[key].ToString();
                    }
                    else
                    {
                        dict = (IDictionary)dict[key];
                    }

                }
                else
                {
                    return null;
                }
            }

            if (str.Length > 0) return str;

            return dict;
        }

        #region Tool
        private static void JsonToDirect(JObject jObject, IDictionary parent_dict)
        {
            foreach (KeyValuePair<String, JToken> item in jObject)
            {
                if (item.Value is JObject)
                {
                    JObject jValue = (JObject)item.Value;

                    IDictionary dict = new Dictionary<String, Object>();
                    parent_dict.Add(item.Key.ToString(), dict);
                    JsonToDirect(jValue, dict);
                }
                else if (item.Value is JArray)
                {
                    JArray jArray = (JArray)item.Value;

                    IList list = new List<Object>();
                    parent_dict.Add(item.Key.ToString(), list);

                    JsonToDirect(jArray, list);
                }
                else
                {
                    parent_dict.Add(item.Key.ToString(), (String)item.Value.ToString());
                }

               
            }
        }

        private static void JsonToDirect(JArray jArray, IList parent_list)
        {
            foreach (Object obj in jArray)
            {
                if (obj is JObject)
                {
                    IDictionary dict = new Dictionary<String, Object>();
                    parent_list.Add(dict);

                    JsonToDirect((JObject)obj, dict);
                }
                else if (obj is JArray)
                {
                    IList list = new List<Object>();
                    parent_list.Add(list);

                    JsonToDirect((JArray)obj, list);
                }
      
            }
        }
        #endregion
    }
}
