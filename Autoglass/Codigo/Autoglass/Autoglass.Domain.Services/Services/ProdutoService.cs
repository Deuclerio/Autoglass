using Autoglass.Domain.Core.Interfaces.Repositories;
using Autoglass.Domain.Core.Interfaces.Services;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using Autoglass.Domain.Services.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Domain.Services.Services
{
    public class ProdutoService : Service<Produto>, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IUnitOfWork context, IProdutoRepository produtoRepository) : base(context, produtoRepository)
        {
            Validator = new ProdutoValidator();
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter)
        {
            return await _produtoRepository.GetByDto(filter);
        }

        public async Task<Produto> GetByIdActive(long id)
        {
            return await _produtoRepository.GetAllActive(id);
        }
    }
}
