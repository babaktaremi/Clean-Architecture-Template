using CleanArc.WebFramework.Validations.Contracts;
using FluentValidation;

namespace CleanArc.WebFramework.Validations;

public class ValidationModelBase<TViewModel> : AbstractValidator<TViewModel> where TViewModel: IValidatableViewModel
{
}