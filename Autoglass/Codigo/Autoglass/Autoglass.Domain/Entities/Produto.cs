using Autoglass.Domain.Base;
using System;

namespace Autoglass.Domain.Entities
{
    public class Produto : Entity
    {
        public string Descricao { get; set; }
        public bool? Situcao { get; set; }
        public DateTime? DataFabricacao { get; set; }

        public DateTime? DataValidade { get; set; }
        public string? CodigoFornecedor { get; set; }
        public string? DescricaoFornecedor { get; set; }
        public string? CnpjFornecedor { get; set; }
    }
}
