﻿using BHJet_Enumeradores;

namespace BHJet_DTO.Dashboard
{
    public class ResumoAtendimentoModel
    {
        public string Mes { get; set; }
        public long? QtdAtendimentoMotorista { get; set; }
        public long? QtdAtendimentoMotociclista { get; set; }
    }
}
