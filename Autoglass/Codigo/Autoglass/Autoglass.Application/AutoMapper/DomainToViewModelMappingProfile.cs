using AutoMapper;
using Autoglass.Application.Dtos;
using Autoglass.Domain.Entities;

namespace Autoglass.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {           
            CreateMap<Produto, ProdutoDto>();
           
        }
    }
}
