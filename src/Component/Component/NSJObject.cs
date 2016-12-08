using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
