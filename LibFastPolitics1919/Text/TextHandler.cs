using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.Text
{
    public static class TextHandler
    {
        public static readonly int CutIndex = 10;
        public static readonly string CutSuffix = "..";

        public static string Cut(object obj)
        {
            return Cut(obj, CutIndex);
        }
        public static string Cut(object obj, int lenght)
        {
            string tmp = obj.ToString();
            if (tmp.Length <= lenght)
                return obj.ToString();
            return tmp.Substring(0, lenght) + CutSuffix;
        }

        //- For sorting
        public static long TextToNumber(string text)
        {
            Int64 hashCode = 0;
            if (!string.IsNullOrEmpty(text))
            {
                //Unicode Encode Covering all characterset
                byte[] byteContents = Encoding.Unicode.GetBytes(text);
                System.Security.Cryptography.SHA256 hash =
                new System.Security.Cryptography.SHA256CryptoServiceProvider();
                byte[] hashText = hash.ComputeHash(byteContents);
                Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
                Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
                Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
                hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
            }
            return hashCode;
        }
    }
}
