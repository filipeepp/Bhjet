using BHJet_Admin.Models;
using BHJet_CoreGlobal;
using BHJet_Enumeradores;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHJet_Admin.Infra
{
    public static class OSAvulsaControle
    {
        //ControllerContext CTR;

        //public OSAvulsaControle(ControllerContext dest)
        //{
        //    CTR = dest;
        //}

        public static readonly string NomeVD = CriptografiaUtil.Criptografa("osavulsa", "ch4v3S3m2nt3o1savu4la");

        public static EntregaModel RetornaOSAvulsa(this Controller context)
        {
            var model = context.TempData[NomeVD] != null ? (EntregaModel)context.TempData[NomeVD] : null;

            if (model != null)
                context.MantemOSAvulsa(model);

            return model;
        }

        public static EntregaModel CriaOSAvulsa(this Controller context, long? idCliente = null)
        {
            // Variaveis
            var entrega = new EntregaModel
            {
                IDCliente = idCliente == null ? Infra.UsuarioLogado.Instance.bhIdCli : idCliente,
                Enderecos = new List<EnderecoModel>()
                 {
                     new EnderecoModel()
                     {
                     }
                 },
                 PassoOS = OSAvulsoPassos.Origem
            };

            // Seta nova Entrega
            context.TempData[NomeVD] = entrega;

            // Return
            return entrega;
        }

        public static void AtualizaOSAvulsa(this Controller context, EntregaModel model)
        {
            context.TempData[NomeVD] = model;
        }

        public static void MantemOSAvulsa(this Controller context, EntregaModel model)
        {
            AtualizaOSAvulsa(context, model);
        }

        public static void MantemOSAvulsa(this Controller context)
        {
            AtualizaOSAvulsa(context, RetornaOSAvulsa(context));
        }

        public static void DeletaOSAvulsa(this Controller context)
        {
            // Busca OS
            var osAvulsa = RetornaOSAvulsa(context);

            // Deleta Entrega
            context.TempData[NomeVD] = new EntregaModel()
            {
                 IDCliente = osAvulsa != null ? osAvulsa.IDCliente : null
            };
        }

        public static EntregaModel FinalizaOrigem(this Controller context, EntregaModel model)
        {
            // Adiciona destino
            model.Enderecos.Add(new EnderecoModel()
            {
            });
            model.Enderecos[0].TipoOcorrencia = model.TipoProfissional == BHJet_Enumeradores.TipoProfissional.Motorista ? 4 : 3;

            // Seta nova Entrega
            AtualizaOSAvulsa(context, model);

            // Return
            return model;
        }
    }

    public interface IOSAvulsaControle
    {
        EntregaModel CriaOSAvulsa(long? idCliente = null);
        EntregaModel RetornaOSAvulsa();
        void AtualizaOSAvulsa(EntregaModel model);
        void DeletaOSAvulsa();
        EntregaModel FinalizaOrigem(EntregaModel model);
    }
}