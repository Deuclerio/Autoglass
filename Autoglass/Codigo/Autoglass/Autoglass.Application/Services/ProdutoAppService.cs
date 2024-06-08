using Autoglass.Application.Base;
using Autoglass.Application.Dtos;
using Autoglass.Application.Service.Interfaces;
using Autoglass.Domain.Core.Interfaces.Repositories;
using Autoglass.Domain.Core.Interfaces.Services;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Application.Services
{
    public class ProdutoAppService : AppService, IProdutoAppService
    {
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly string _nomeTela = "Produto";

        public ProdutoAppService(IUnitOfWork uoW, 
                                 IMapper mapper,
                                 IProdutoService produtoService) : base(uoW, mapper)
        {
            _mapper = mapper;
            _produtoService= produtoService;
        }

        public async Task<IEnumerable<Produto>> GetByDto(ProdutoFilter filter)
        {
            try
            {
                var listaProduto = await _produtoService.GetByDto(filter);
                var listaProdutoDto = _mapper.Map<IEnumerable<Produto>>(listaProduto);

                return listaProdutoDto;
            }
            catch (Exception ex)
            {
                Error(ex, "GetByDto");
                throw;
            }
        }

        public async Task<IEnumerable<ProdutoDto>> GetAllActive()
        {
            try
            {
                var listaProduto = await _produtoService.GetAll();
                var listaProdutoDto = _mapper.Map<IEnumerable<ProdutoDto>>(listaProduto);

                return listaProdutoDto;
            }
            catch (Exception ex)
            {
                Error(ex, "GetAllActive");
                throw;
            }
        }

        public async Task<ProdutoDto> GetById(long Id)
        {
            var produto = await _produtoService.GetByIdActive(Id);
            return _mapper.Map<ProdutoDto>(produto);
        }

        public async Task Inactivate(long id)
        {
            try
            {
                UoW.BeginTransaction();
                var produtoBase = await _produtoService.GetByIdActive(id);
                if (produtoBase == null)
                {
                    throw new Exception("Produto não encontrado.");
                }

                produtoBase.Situcao = false;
                await _produtoService.Inactivate(Mapper.Map<Produto>(produtoBase));

                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                Error(ex, "Inactivate");
                throw ex;
            }
        }

        public async Task<ProdutoDto> Insert(ProdutoDto obj)
        {
            try
            {

                UoW.BeginTransaction();

                ValidationDate(obj.DataFabricacao, obj.DataValidade);

                var produto = Mapper.Map<Produto>(obj);
                var produtoSave = await _produtoService.Insert(Mapper.Map<Produto>(produto));


                UoW.Commit();

                return Mapper.Map<ProdutoDto>(produtoSave);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                Error(ex, "Insert");
                throw;
            }
        }

        public static void ValidationDate(DateTime? dataFabricacao, DateTime?  dataValidade)
        {
            //Criar validação para o campo data de fabricação que não poderá ser maior ou igual a data de validade.
            if (dataFabricacao >= dataValidade)
            {
                throw new Exception("Data de fabricação é maior ou igual à data de validade");
            }
        }

        public async Task<ProdutoDto> Update(long id, ProdutoDto obj)
        {
            try
            {
                ValidationDate(obj.DataFabricacao, obj.DataValidade);

                UoW.BeginTransaction();
                var produtoBase = await _produtoService.GetByIdActive(id);
                if (produtoBase == null)
                {
                    throw new Exception("Produto não encontrado.");
                }

                produtoBase.Id = id;
                produtoBase.Descricao = obj.Descricao;
                produtoBase.Situcao = obj.Situcao;

                produtoBase.DataFabricacao = obj.DataFabricacao;
                produtoBase.DataValidade = obj.DataValidade;

                produtoBase.CodigoFornecedor = obj.CodigoFornecedor;
                produtoBase.DescricaoFornecedor = obj.DescricaoFornecedor;
                produtoBase.CnpjFornecedor = obj.CnpjFornecedor;

                var produto = await _produtoService.Update(produtoBase);


                UoW.Commit();
                return Mapper.Map<ProdutoDto>(produto);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                Error(ex, "Update");
                throw;
            }
        }

        private void Error(Exception ex, string acao)
        {
            _produtoService.showError(_nomeTela, ex?.Message);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
               
    }
}
