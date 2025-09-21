using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public static class CountValidator
    {
        public static int Validate(int value) => Mathf.Clamp(value, 0, int.MaxValue);
        /// <summary>
        /// returns value was changed
        /// </summary>
        public static bool Validate(int count, out int value)
        {
            value = Mathf.Clamp(count, 0, int.MaxValue);
            return value != count;
        }

        /// 0-999M+
        public static string ToShortText(int value)
            => Validate(value) switch
            {
                < 0 => "-" + ToShortText(-value),
                < 1000 => value.ToString(),
                < 1000000 => $"{value / 1000}K",
                < 1000000000 => $"{value / 1000000}M",
                _ => "999M+"
            };
    }
}
