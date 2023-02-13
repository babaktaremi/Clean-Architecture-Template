using FluentValidation;

namespace CleanArc.Application.Common.ValidationBase.Contracts;

public interface IValidatableApplicationModel<in TApplicationModel> where TApplicationModel:class,new()
{
    IValidator<TApplicationModel> ValidateApplicationModel();
}