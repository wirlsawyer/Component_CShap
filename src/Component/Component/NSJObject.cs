using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Component
{
    public class NSJObject
    {
        private Dictionary<String, Object> mDict = new Dictionary<string, object>();

        public NSJObject()
        {

        }
        public NSJObject(String propertyName, String value)
        {
            this.Add(propertyName, value);
        }
        public NSJObject(String propertyName, Object value)
        {
            this.Add(propertyName, value);
        }

        public void Add(String propertyName, Object value)
        {
            mDict.Add(propertyName, value);
        }

        public String ToJson()
        {
            String result = "{";

            foreach (KeyValuePair<String, Object> item in mDict)
            {
                if (result.Length > 1)
                {
                    result += ",";
                }

                switch (item.Value.GetType().Name)
                {

                    case "NSJArray":
                        result += String.Format("\"{0}\":{1}", item.Key, (item.Value as NSJArray).ToJson());
                        break;
                    case "NSJObject":
                        result += String.Format("\"{0}\":{1}", item.Key, (item.Value as NSJObject).ToJson());
                        break;
                    case "int":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "double":
                    case "Double":
                    case "long":
                        result += String.Format("\"{0}\":{1}", item.Key, item.Value);
                        break;
                    case "bool":
                    case "Boolean":
                        result += String.Format("\"{0}\":\"{1}\"", item.Key, item.Value.ToString());
                        break;
                    case "String":
                        result += String.Format("\"{0}\":\"{1}\"", item.Key, item.Value);
                        break;
                    default:
                        result += String.Format("\"{0}\":\"{1}\"", item.Key, item.Value.ToString());
                        break;
                }
            }

            result += "}";
            return result;
        }

        #region Parser
        static public Object FastGet(Dictionary<String, Object> dict, params Object[] Params)
        {
            Object Source = dict;
            foreach (Object key in Params)
            {
                Object value;
                if (Source.GetType() == typeof(Dictionary<String, Object>))
                {
                    if (((Dictionary<String, Object>)Source).TryGetValue(key.ToString(), out value))
                    {
                        Source = value;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (Source.GetType() == typeof(List<Object>))
                {
                    if (((List<Object>)Source).Count > int.Parse(key.ToString()))
                    {
                        value = ((List<Object>)Source).ElementAt(int.Parse(key.ToString()));
                        Source = value;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return Source;
        } 
        static private int ParserArray(String json, String curKey, Dictionary<String, Object> dict)
        {
            int iPos1 = -1;
            int iPos2 = -1;
            int iPos5 = 0;
            String curValue = "";
            List<Object> list = new List<Object>();
            dict.Add(curKey, list);

            for (int i = 0; i < json.Length; i++)
            {
                switch (json.ElementAt(i))
                {
                    case '[':
                        i += ParserArray(json.Substring(i + 1), curKey, dict);
                        break;
                    case '{':
                        Dictionary<String, Object> newDict = new Dictionary<String, Object>();
                        list.Add(newDict);
                        i += ParserObject(json.Substring(i + 1), newDict);
                        break;
                    case '"':
                        if (iPos1 == -1)
                        {
                            iPos1 = i + 1;
                        }
                        else
                        {
                            iPos2 = i;
                        }
                        break;

                    case ',':
                    case ']':
                        if (iPos1 < iPos2)
                        {
                            curValue = json.Substring(iPos1, iPos2 - iPos1);
                            list.Add(curValue);
                            //Console.WriteLine("key:" + curKey + " value:" + curValue);
                        }
                        else if (iPos1 == -1 && iPos2 == -1)
                        {
                            curValue = json.Substring(iPos5, i - iPos5);
                            list.Add(curValue);
                            //Console.WriteLine("key:" + curKey + " value:" + curValue);
                        }

                        iPos1 = -1;
                        iPos2 = -1;
                        iPos5 = i + 1;
                        break; 
                        
                }
            }
            return iPos5;
        }
        static private int ParserObject(String json, Dictionary<String, Object> dict)
        {
            int iPos1 = -1;
            int iPos2 = -1;
            int iPos3 = -1;
            int iPos4 = -1;

            String curKey = "";
            String curValue = "";
            for (int i = 0; i < json.Length; i++)
            {
                switch (json.ElementAt(i))
                {
                    case '[':
                        i += ParserArray(json.Substring(i + 1), curKey, dict);
                        iPos3 = -1;
                        break;
                    case '{':
                        i += ParserObject(json.Substring(i + 1), dict);
                        break;
                    case ']':
                        break;
                    

                    case '"':
                        if (iPos1 == -1)
                        {
                            iPos1 = i+1;
                        }
                        else
                        {
                            iPos2 = i;
                        }
                        break;
                    case ':':
                        if (iPos1 < iPos2)
                        {
                            curKey = json.Substring(iPos1, iPos2 - iPos1);
                            iPos1 = -1;
                            iPos2 = -1;
                        }
                        iPos3 = i + 1;
                        break;
                    case ',':
                    case '}':
                        if (iPos1 < iPos2)
                        {
                            iPos4 = iPos2;
                            // string
                            curValue = json.Substring(iPos1, iPos2 - iPos1);
                            iPos1 = -1;
                            iPos2 = -1;
                            //Console.WriteLine("key:"+ curKey + " value:"+ curValue);
                            if (dict.ContainsKey(curKey))
                            {
                            }
                            else
                            {
                                dict.Add(curKey, curValue);
                            }
                            
                        }
                        else if (iPos1 == -1 && iPos2 == -1)
                        {
                            iPos4 = i;

                            if (iPos3 == -1) break;
                            // numic
                            curValue = json.Substring(iPos3, i - iPos3);
                            //Console.WriteLine("key:" + curKey + " value:" + curValue);
                            if (dict.ContainsKey(curKey))
                            {
                            }
                            else
                            {
                                dict.Add(curKey, curValue);
                            }
                        }
                        break;
                }
            }

            return iPos4+1;
        }
        static public Dictionary<String, Object> Parser(String json)
        {
            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            
            for (int i = 0; i < json.Length; i++)
            {
                switch (json.ElementAt(i))
                {
                    case '{':
                        i += ParserObject(json.Substring(i + 1), dict);
                        break;

                    case '[':
                       // i += ParserArray(json.Substring(i + 1), "[root]", dict);
                        break;
                }
            }
            /*
            foreach (KeyValuePair<String, Object> item in dict)
            {
                if (item.Value.GetType() == typeof(List<Object>))
                {
                    Console.Write(item.Key + " : ");
                    foreach (Object o in (List<Object>)item.Value)
                    {
                        Console.Write(o.ToString() + ", ");
                    }
                    Console.Write("\r\n");
                }
                else
                {
                    Console.WriteLine(string.Format("{0} : {1}", item.Key, item.Value));
                }
                
            }
            */
            return dict;
        }

       
        #endregion
    }
}
