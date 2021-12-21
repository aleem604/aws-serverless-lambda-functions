using System;
using TinCore.Common.Enums;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class UpdateLocationCommand : LocationCommand
    {       
        public override bool IsValid()
        {
            ValidationResult = new UpdateLocationCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}