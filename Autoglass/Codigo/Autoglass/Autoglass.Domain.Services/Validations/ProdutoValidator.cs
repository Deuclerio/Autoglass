using Autoglass.Domain.Entities;
using FluentValidation;

namespace Autoglass.Domain.Services.Validations
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(o => o.Descricao)
                .NotEmpty().WithMessage("Nome é Obrigatório");
        }
    }
}
