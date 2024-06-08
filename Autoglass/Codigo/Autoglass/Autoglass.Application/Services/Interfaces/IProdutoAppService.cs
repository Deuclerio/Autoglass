using Autoglass.Application.Dtos;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Application.Service.Interfaces
{
    public interface IProdutoAppService : IDisposable
    {
        Task<ProdutoDto> Insert(ProdutoDto obj);
        Task<ProdutoDto> Update(long id, ProdutoDto obj);
        Task Inactivate(long id);
        Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter);
        Task<ProdutoDto> GetById(long Id);
    }
}
