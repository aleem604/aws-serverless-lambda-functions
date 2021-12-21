using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class RemoveLocationCommand : LocationCommand
    {       
        public override bool IsValid()
        {
            ValidationResult = new RemoveLocationCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}