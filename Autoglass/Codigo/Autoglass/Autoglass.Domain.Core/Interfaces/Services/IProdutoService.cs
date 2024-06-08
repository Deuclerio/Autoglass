using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Domain.Core.Interfaces.Services
{
    public interface IProdutoService : IService<Produto>
    {
        Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter);
        Task<Produto> GetByIdActive(long? id);
    }
}
