using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BHJet_Mobile.Infra.Extensao
{
    public static class DatetimeExtensao
    {
        public static bool IsValidTimeFormat(this string input)
        {
            return Regex.IsMatch(input, @"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
        }
    }
}
