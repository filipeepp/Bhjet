using System;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Dashboard
{
    public class DiariaModel
    {
        private DateTime _PeriodoInicial;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PeriodoInicial
        {
            get
            {
                return _PeriodoInicial;
            }
            set
            {
                _PeriodoInicial = value;
            }
        }

    }
}