using CleanArc.Application.Common.ValidationBase.Contracts;
using FluentValidation;
using Mediator;

namespace CleanArc.Application.Common.ValidationBase;

public class ApplicationBaseValidationModel<TApplicationModel>:AbstractValidator<TApplicationModel>
{
    
}