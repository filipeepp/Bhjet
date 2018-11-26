using System;
using System.Globalization;
using System.Text.RegularExpressions;

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
            string[] split = Regex.Split(valor, @"(?<!^)(?=[A-Z])");
            return string.Join(" ", split);
        }

        /// <summary>
        /// Converte para Decimal padrão moeda
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static decimal ToDecimalCurrency(this string valor)
        {
            var result = default(decimal);
            if (decimal.TryParse(valor.Replace("R$", "").Replace(" ", ""), NumberStyles.Currency, new CultureInfo("pt-BR"), out result))
                return result;
            return 0;
        }

        /// <summary>
        /// Converte para Long
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static long ToLong(this string valor)
        {
            var result = default(long);
            if (long.TryParse(valor.Replace(" ", ""), out result))
                return result;
            return 0;
        }

        /// <summary>
        /// Converte para DateTime
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string valor)
        {
            var result = default(DateTime);
            if (DateTime.TryParse(valor, out result))
                return result;
            return null;
        }


    }
}
