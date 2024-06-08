using System;

namespace Autoglass.Domain.Filter
{
    public class ProdutoFilter
    {
        public string Descricao { get; set; } = string.Empty;
        public bool? Situcao { get; set; }
        public DateTime? DataFabricacao { get; set; }

        public DateTime? DataValidade { get; set; }
        public string? CodigoFornecedor { get; set; }
        public string? DescricaoFornecedor { get; set; }
        public string? CnpjFornecedor { get; set; }
    }
}
