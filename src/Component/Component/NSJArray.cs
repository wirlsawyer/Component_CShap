using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class NSJArray
    {
        private ArrayList mData = new ArrayList();
        public NSJArray()
        {

        }

        public NSJArray(params Object[] args)
        {
            this.Add(args);
        }

        public void Add(params Object[] args)
        {
            foreach (Object value in args)
            {
                mData.Add(value);
            }
        }

        public String ToJson()
        {
            String result = "[";
 
            foreach (Object value in mData)
            {
                if (result.Length > 1)
                {
                    result += ",";
                }

                switch (value.GetType().Name)
                {

                    case "NSJArray":

                        break;
                    case "NSJObject":
                        result += (value as NSJObject).ToJson();
                        break;
                    case "int":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "double":
                    case "Double":
                    case "long":
                        result += String.Format("{0}", value);
                        break;
                    case "bool":
                    case "Boolean":
                        result += String.Format("\"{0}\"", value.ToString());
                        break;
                    case "String":
                        result += String.Format("\"{0}\"", value);
                        break;
                    default:
                        result += String.Format("\"{0}\"", value.ToString());
                        break;
                }
            }

            result += "]";
            return result;
        }
    }
}
