using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
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

            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(json);
                jsonToDirect(jObject, dict);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                return false;
            }

            return true;
        }

        private static void EnumDirect(IDictionary parent_dict)
        {
            foreach (String key in parent_dict.Keys)
            {
                Object value = parent_dict[key];

                if (parent_dict[key].GetType() == typeof(String))
                {
                    Console.WriteLine(key + " : " + value.ToString());
                }
                else
                {
                    Console.WriteLine(key);

                    EnumDirect((IDictionary)value);
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
        private static void jsonToDirect(JObject jObject, IDictionary parent_dict)
        {
            foreach (KeyValuePair<String, JToken> item in jObject)
            {
                try
                {
                    //Console.WriteLine(item.Key);
                    JObject jValue = (JObject)item.Value;

                    IDictionary dict = new Dictionary<String, Object>();
                    parent_dict.Add(item.Key.ToString(), dict);
                    JsonToDirect(jValue, dict);
                }
                catch (Exception e)
                {
                    parent_dict.Add(item.Key.ToString(), (String)item.Value.ToString());
                    //Console.WriteLine(item.Value.ToString());
                }
            }
        }
        #endregion
    }
}
