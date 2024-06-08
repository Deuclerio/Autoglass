using System;
using System.ComponentModel.DataAnnotations;

namespace Autoglass.Application.Dtos
{
    public class ProdutoDto
    {
        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string Descricao { get; set; }
        public bool? Situcao { get; set; }

        public string SituacaoDescricao
        {
            get
            {
                return Situcao.HasValue ? (Situcao.Value ? "Ativo" : "Inativo") : "Desconhecido";
            }
        }
        public DateTime? DataFabricacao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? CodigoFornecedor { get; set; }
        public string? DescricaoFornecedor { get; set; }
        public string? CnpjFornecedor { get; set; }
    }
}
