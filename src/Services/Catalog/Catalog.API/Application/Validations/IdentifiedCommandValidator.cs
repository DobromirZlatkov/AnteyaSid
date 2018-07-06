namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Validations
{
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using FluentValidation;

    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<UpdateCatalogItemCommand, CatalogItem>>
    {
        public IdentifiedCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
