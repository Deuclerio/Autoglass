using AutoMapper;
using Autoglass.Application.Dtos;
using Autoglass.Domain.Entities;

namespace Autoglass.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProdutoDto, Produto>();
        }
    }
}
