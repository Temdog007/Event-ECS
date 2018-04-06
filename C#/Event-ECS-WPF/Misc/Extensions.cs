using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_ECS_Client_WPF
{
    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, int length)
        {
            for(int i = 0, n = str.Length; i < n; i += length )
            {
                if(i + length >= n)
                {
                    yield return str.Substring(i);
                }
                else
                {
                    yield return str.Substring(i, Math.Min(length, n - 1));
                }
            }
        }
    }
}
