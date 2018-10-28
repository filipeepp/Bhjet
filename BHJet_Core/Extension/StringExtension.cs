﻿using System.Text.RegularExpressions;

namespace BHJet_Core.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Separa string por letra maiuscula
        /// Ex: TestandoString -> Testando String
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string UpperCaseSeparete(this string valor)
        {
            var teste = "521732131";
            var teste2 = "1313";
            string[] split = Regex.Split(valor, @"(?<!^)(?=[A-Z])");
            return string.Join(" ", split);
        }



    }
}
