using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Component
{
    public class NSRegex
    {
        static public String Do(String input, String pattern)
        {
            //string input = mUart.Result.ToString();
            //string pattern = @"\d{1,2}%$";
            //宣告 Regex 忽略大小寫
            String result = "";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(input))
            {
                //將比對後集合傳給 MatchCollection
                MatchCollection matches = regex.Matches(input);
                foreach (Match match in matches)
                {
                    // 將 Match 內所有值的集合傳給 GroupCollection groups
                    GroupCollection groups = match.Groups;
                    // 印出 Group 內 word 值
                    result = groups.SyncRoot.ToString();
                }
            }

            return result;
        }
    }
}
