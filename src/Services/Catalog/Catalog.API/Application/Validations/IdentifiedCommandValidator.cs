namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Validations
{
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using FluentValidation;

    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateCatalogItemCommand, int>>
    {
        public IdentifiedCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
