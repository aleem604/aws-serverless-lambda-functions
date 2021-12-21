using System;
using TinCore.Common.Enums;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class NewLocationCommand : LocationCommand
    {        
        public override bool IsValid()
        {
            ValidationResult = new NewLocationCommandValidation().Validate(this);
            return ValidationResult.IsValid;            
        }
    }
}