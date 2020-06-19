using System;
using System.Text;

namespace TOFunction.Extensions
{
    public static class StringExtensions
    {
        public static string DecodeBase64(this string text)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncodeBase64(this string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
