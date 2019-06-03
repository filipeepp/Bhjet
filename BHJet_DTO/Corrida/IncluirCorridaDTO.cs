using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_DTO.Corrida
{
    public class IncluirCorridaDTO
    {
        public long? IDCliente { get; set; }
        public long? IDProfissional { get; set; }
        public int? TipoProfissional { get; set; }
        public List<EnderecoCorridaDTO> Enderecos { get; set; }
    }

    public class EnderecoCorridaDTO
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Descricao { get; set; }
        public string ProcurarPessoa { get; set; }
        public int? TipoOcorrencia { get; set; }
        public string Observacao { get; set; }
    }
}
