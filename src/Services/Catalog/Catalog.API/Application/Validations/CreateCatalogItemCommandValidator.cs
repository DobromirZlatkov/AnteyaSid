namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Validations
{
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using FluentValidation;

    public class CreateCatalogItemCommandValidator : AbstractValidator<UpdateCatalogItemCommand>
    {
        public CreateCatalogItemCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Color).NotEmpty();
            RuleFor(command => command.Price).NotEmpty().Must(PriceMustBePossitiveNumber).WithMessage("Price cannot be smaller than 0");
        }

        private bool PriceMustBePossitiveNumber(decimal price)
        {
            return price > 0;
        }
    }
}
