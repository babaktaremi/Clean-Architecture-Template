using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArc.WebFramework.Validations.Contracts
{
    public interface IValidatableViewModel
    {
        ValidationResult ValidateRules();
    }
}
