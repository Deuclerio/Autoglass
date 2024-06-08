using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Domain.Core.Interfaces.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<Produto> GetAllActive(long? id);
        Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter);
    }
}
