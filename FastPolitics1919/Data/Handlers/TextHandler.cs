using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.Handlers
{
    public static class TextHandler
    {
        public static string ToFormatNumber(int num)
        {
            string tmp = num.ToString();
            char[] tmp_chars = tmp.ToCharArray();
            List<char> char_list = new List<char>();
            
            //- 0 1 2 3 4 5 6 7 => 8
            //- 1 0 0 0 0 0 0 0
            for (int i = (tmp_chars.Length - 1); i > (0 - 1); i--)
            {
                char_list.Add(tmp_chars[i]);
                if ((i - 1) % 3 == 0)
                    char_list.Add('.');
            }
            char_list.Reverse();
            return new string(char_list.ToArray());
        }
    }
}
