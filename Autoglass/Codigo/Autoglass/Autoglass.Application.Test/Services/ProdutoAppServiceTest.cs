using Autoglass.Application.Dtos;
using Autoglass.Application.Services;
using Autoglass.Domain.Core.Interfaces.Repositories;
using Autoglass.Domain.Core.Interfaces.Services;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Filter;
using AutoMapper;
using Moq;
using Xunit;

namespace Autoglass.Application.Test.Services
{
    public class ProdutoAppServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IProdutoService> _mockProdutoService;
        private readonly ProdutoAppService _produtoAppService;
        public ProdutoAppServiceTest()
        {
            _mockUoW = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProdutoService = new Mock<IProdutoService>();
            _produtoAppService = new ProdutoAppService(_mockUoW.Object, _mockMapper.Object, _mockProdutoService.Object);

        }

        [Fact]
        public async Task GetByDto_ReturnsMappedProducts()
        {
            // Arrange
            var filter = new ProdutoFilter { Descricao = "Produto" };
            var productList = new List<Produto>
        {
            new Produto { Id = 1, Descricao = "Produto 1", Situcao = true },
            new Produto { Id = 2, Descricao = "Produto 2",Situcao = true }
        };

            _mockProdutoService.Setup(service => service.GetByDto(filter)).ReturnsAsync(productList);

            // Act
            var result = await _produtoAppService.GetByDto(filter);

            // Assert
            Assert.NotNull(result);
            _mockProdutoService.Verify(service => service.GetByDto(filter), Times.Once);
        }

        [Fact]
        public async Task GetByDto_ThrowsException_WhenServiceFails()
        {
            // Arrange
            var filter = new ProdutoFilter { /* configure filter properties if needed */ };
            _mockProdutoService.Setup(service => service.GetByDto(filter)).ThrowsAsync(new Exception("Service failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _produtoAppService.GetByDto(filter));
            _mockProdutoService.Verify(service => service.GetByDto(filter), Times.Once);
        }

        [Fact]
        public async Task GetAllActive_ReturnsMappedProducts()
        {
            // Arrange
            var products = new List<Produto> { new Produto { Id = 1, Descricao = "Product 1" } };
            var productDtos = new List<ProdutoDto> { new ProdutoDto { Descricao = "Product 1" } };
            _mockProdutoService.Setup(s => s.GetAll()).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProdutoDto>>(products)).Returns(productDtos);

            // Act
            var result = await _produtoAppService.GetAllActive();

            // Assert
            Assert.Equal(productDtos, result);
        }

        [Fact]
        public async Task GetAllActive_ThrowsException_WhenServiceThrowsException()
        {
            // Arrange
            _mockProdutoService.Setup(s => s.GetAll()).ThrowsAsync(new Exception("Service exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _produtoAppService.GetAllActive());
            Assert.Equal("Service exception", exception.Message);
        }



        [Fact]
        public async Task GetById_ReturnsNull_WhenProdutoDoesNotExist()
        {
            // Arrange
            var produtoId = 1;

            _mockProdutoService.Setup(s => s.GetByIdActive(produtoId)).ReturnsAsync((Produto)null);

            // Act
            var result = await _produtoAppService.GetById(produtoId);

            // Assert
            Assert.Null(result);
            _mockProdutoService.Verify(s => s.GetByIdActive(produtoId), Times.Once);
        }

        [Fact]
        public async Task Inactivate_ShouldInactivateProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var produto = new Produto { Id = productId, Situcao = true };
            _mockProdutoService.Setup(x => x.GetByIdActive(productId)).ReturnsAsync(produto);
            _mockMapper.Setup(m => m.Map<Produto>(It.IsAny<Produto>())).Returns(produto);

            // Act
            await _produtoAppService.Inactivate(productId);

            // Assert
            _mockProdutoService.Verify(x => x.Inactivate(It.IsAny<Produto>()), Times.Once);
            _mockUoW.Verify(x => x.Commit(), Times.Once);
            _mockUoW.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var produtoDto = new ProdutoDto { Descricao = "Teste" /*... properties ...*/ };
            var produtoBase = new Produto { Id = productId, /*... properties ...*/ };
            var produtoUpdated = new Produto { Id = productId, /*... properties ...*/ };

            _mockProdutoService.Setup(x => x.GetByIdActive(productId)).ReturnsAsync(produtoBase);
            _mockProdutoService.Setup(x => x.Update(produtoBase)).ReturnsAsync(produtoUpdated);
            _mockMapper.Setup(m => m.Map<ProdutoDto>(produtoUpdated)).Returns(produtoDto);

            // Act
            var result = await _produtoAppService.Update(productId, produtoDto);

            // Assert
            Assert.Equal(produtoDto, result);
            _mockUoW.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Insert_ShouldThrowException_WhenDataFabricacaoIsGreaterThanOrEqualDataValidade()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Descricao = "Teste",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now.AddDays(-1)
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _produtoAppService.Insert(produtoDto));
            _mockUoW.Verify(x => x.Rollback(), Times.Once);
        }


        [Fact]
        public async Task Inactivate_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var productId = 1;
            _mockProdutoService.Setup(x => x.GetByIdActive(productId)).ReturnsAsync((Produto)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _produtoAppService.Inactivate(productId));
            _mockUoW.Verify(x => x.Rollback(), Times.Once);
        }


        [Fact]
        public async Task Update_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var productId = 1;
            var produtoDto = new ProdutoDto { Descricao = "Teste" };
            _mockProdutoService.Setup(x => x.GetByIdActive(productId)).ReturnsAsync((Produto)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _produtoAppService.Update(productId, produtoDto));
            _mockUoW.Verify(x => x.Rollback(), Times.Once);
        }


        [Fact]
        public void ValidationDate_ShouldThrowException_WhenDataFabricacaoIsGreaterThanDataValidade()
        {
            // Arrange
            DateTime dataFabricacao = new DateTime(2023, 1, 1);
            DateTime dataValidade = new DateTime(2022, 12, 31);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => ProdutoAppService.ValidationDate(dataFabricacao, dataValidade));
            Assert.Equal("Data de fabricação é maior ou igual à data de validade", exception.Message);
        }

        [Fact]
        public void ValidationDate_ShouldThrowException_WhenDataFabricacaoIsEqualToDataValidade()
        {
            // Arrange
            DateTime dataFabricacao = new DateTime(2023, 1, 1);
            DateTime dataValidade = new DateTime(2023, 1, 1);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => ProdutoAppService.ValidationDate(dataFabricacao, dataValidade));
            Assert.Equal("Data de fabricação é maior ou igual à data de validade", exception.Message);
        }

        [Fact]
        public void ValidationDate_ShouldNotThrowException_WhenDataFabricacaoIsLessThanDataValidade()
        {
            // Arrange
            DateTime dataFabricacao = new DateTime(2022, 12, 31);
            DateTime dataValidade = new DateTime(2023, 1, 1);

            // Act & Assert
            var exception = Record.Exception(() => ProdutoAppService.ValidationDate(dataFabricacao, dataValidade));
            Assert.Null(exception);
        }
    }
}

