using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Core.Utilitario
{
    public static class MedidaUtil
    {


        public static double MetroParaKM(this double metros)
        {
            return Math.Round(metros / 1000, 2);
        }


    }
}
