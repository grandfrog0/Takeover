using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Instruments
{
    public static class TimeValidator
    {
        /// 0000:00:00
        public static string ToShortText(int value)
        {
            int hours = value / 3600;
            int minutes = (value - hours * 3600) / 60;
            int seconds = value - hours * 3600 - minutes * 60;
            return hours > 0 ? $"{hours}:{minutes:00}:{seconds:00}" : $"{minutes:00}:{seconds:00}";
        }
    }
}
