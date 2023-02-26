using FluentValidation;

namespace CleanArc.SharedKernel.ValidationBase.Contracts;

public interface IValidatableModel<TApplicationModel> where TApplicationModel:class
{
    IValidator<TApplicationModel> ValidateApplicationModel(ApplicationBaseValidationModel<TApplicationModel> validator);
}