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

        private int ParserArray(String json, String curKey, Dictionary<String, Object> dict)
        {
            int iPos1 = 0;
            String curValue = "";

            for (int i = 0; i < json.Length; i++)
            {
                switch (json.ElementAt(i))
                {
                    case '[':
                        i += ParserArray(json.Substring(i + 1), curKey, dict);
                        break;
                    case '{':
                       // i += ParserObject(json.Substring(i + 1), dict);
                        break;

                    case ',':
                    case ']':
                        curValue = json.Substring(iPos1, i - iPos1);
                        Console.WriteLine("key:" + curKey + " value:" + curValue);
                        iPos1 = i + 1;
                        break; 
                        
                }
            }
            return iPos1;
        }
        private int ParserObject(String json, Dictionary<String, Object> dict)
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
                            Console.WriteLine("key:"+ curKey + " value:"+ curValue);
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
                            // numic
                            curValue = json.Substring(iPos3, i - iPos3);
                            Console.WriteLine("key:" + curKey + " value:" + curValue);
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
        #region Parser
        public NSJObject Parser(String json)
        {
        

            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            
            for (int i = 0; i < json.Length; i++)
            {
                switch (json.ElementAt(i))
                {
                    case '{':
                        json = json.Substring(i+1);
                        ParserObject(json, dict);
                        break;
                    
                    case '"':
                    case ':':
                    case ',':                       
                    case '}':
                        break;
                }
            }

            foreach (KeyValuePair<String, Object> item in dict)
            {
                Console.WriteLine(string.Format("{0} : {1}", item.Key, item.Value));
            }
            return null;
            NSJObject result = null;
            if (json.Substring(0, 1) != "{")
            {
                return result;
            }

            String[] items = json.Split(',');
            foreach (String item in items)
            {
                String pattern1 = @"[\'\""]((.*?)[\'\""\s][:][\s\'\""])";
                String pattern2 = @"([\s:\s][\'\""](.*?)[\'\""])";
                String pattern3 = @"([\'\""\s][:][\s\'\""])";


                String key = NSRegex.Do(item, pattern1);
                if (key.Length == 0)
                {
                    pattern1 = @"[\'\""]((.*?)[\'\""\s][:\s])";
                    pattern2 = @"([\s:\s](.*?)*)";
                    pattern3 = @"([\'\""\s][:\s])";
                    key = NSRegex.Do(item, pattern1);
                }
                
                String spl = NSRegex.Do(item, pattern3);
                String value = item.Replace(key+spl, "");

                key = key.Substring(1, key.Length - 1 - spl.Length);
                value = value.Substring(spl.Length-1, value.Length - 1 - spl.Length);

                if (result == null)
                {
                    result = new NSJObject();
                }
                result.Add(key, value);                
            }

            return result;
        }
        #endregion
    }
}
