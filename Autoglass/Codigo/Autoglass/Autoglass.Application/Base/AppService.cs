using AutoMapper;
using Autoglass.Domain.Core.Interfaces.Repositories;

namespace Autoglass.Application.Base
{
    public abstract class AppService(IUnitOfWork uoW, IMapper Mapper)
    {
        protected IUnitOfWork UoW { get; set; } = uoW;
        protected IMapper Mapper { get; set; } = Mapper;
       
    }
}
