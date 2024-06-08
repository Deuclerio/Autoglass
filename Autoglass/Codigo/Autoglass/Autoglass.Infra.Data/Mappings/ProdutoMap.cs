using Autoglass.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autoglass.Infra.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Descricao);
            builder.Property(p => p.Situcao);
            builder.Property(p => p.DataFabricacao);
            builder.Property(p => p.DataValidade);
            builder.Property(p => p.CodigoFornecedor);
            builder.Property(p => p.DescricaoFornecedor);
            builder.Property(p => p.CnpjFornecedor);
        }
    }
}
