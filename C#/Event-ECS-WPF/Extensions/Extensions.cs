using System;
using System.Collections.Generic;

namespace Event_ECS_WPF.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, uint length)
        {
            for(uint i = 0, n = (uint)str.Length; i < n; i += length )
            {
                if(i + length >= n)
                {
                    yield return str.Substring((int)i);
                }
                else
                {
                    yield return str.Substring((int)i, (int)Math.Min(length, n - 1));
                }
            }
        }
    }
}
