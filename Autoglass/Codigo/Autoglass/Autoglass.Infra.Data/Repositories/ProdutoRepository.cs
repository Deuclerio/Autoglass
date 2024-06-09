using Autoglass.Domain.Core.Interfaces.Repositories;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoglass.Infra.Data.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(IUnitOfWork context) : base(context)
        {
        }

        public async Task<Produto> GetAllActive(long id)
        {
            var query = await Uow.Context.Set<Produto>().Where(p => p.Id == id && p.Situcao == true).FirstOrDefaultAsync();
            return query;
        }

        public async Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter)
        {
            var predicate = PredicateBuilder.True< Produto>(); // Start with an 'All' predicate

            if (!string.IsNullOrEmpty(filter.Descricao))
            {
                predicate = predicate.And(p => p.Descricao.Contains(filter.Descricao));
            }

            if (filter.Situcao.HasValue)
            {
                predicate = predicate.And(p => p.Situcao == filter.Situcao);
            }

            if (filter.DataFabricacao.HasValue)
            {
                predicate = predicate.And(p => p.DataFabricacao == filter.DataFabricacao);
            }

            if (filter.DataValidade.HasValue)
            {
                predicate = predicate.And(p => p.DataValidade == filter.DataValidade);
            }

            if (!string.IsNullOrEmpty(filter.CodigoFornecedor))
            {
                predicate = predicate.And(p => p.CodigoFornecedor.Contains(filter.CodigoFornecedor));
            }

            if (!string.IsNullOrEmpty(filter.DescricaoFornecedor))
            {
                predicate = predicate.And(p => p.DescricaoFornecedor.Contains(filter.DescricaoFornecedor));
            }

            if (!string.IsNullOrEmpty(filter.CnpjFornecedor))
            {
                predicate = predicate.And(p => p.CnpjFornecedor.Contains(filter.CnpjFornecedor));
            }

            var query = Uow.Context.Set<Produto>().AsNoTracking().Where(predicate);

            return await query.ToListAsync();
        }
    }
}