using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Core.Utilitario
{
    public static class DataUtil
    {
        /// <summary>
        /// Busca SelectListItem Numero e Nome do mês 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ListaMesesAno()
        {
            var listaData = DateTimeFormatInfo
                   .GetInstance(new CultureInfo("pt-BR"))
                   .MonthNames
                   .Select((monthName, index) => new SelectListItem
                   {
                       Value = (index + 1).ToString(),
                       Text = monthName.ToUpper()
                   });

            return listaData.Take(12);
        }

        /// <summary>
        /// Busca lista dos X proximos anos, incluindo ano anterior
        /// </summary>
        /// <param name="qtdAnos">Quantidade de anos seguintes</param>
        /// <returns></returns>
        public static IEnumerable<int> BuscaProximosAnos(int qtdAnos)
        {
            return Enumerable.Range(DateTime.Now.Year - 1, qtdAnos);
        }

    }
}
