using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class UpdateEntityCommand : EntityCommand
    {

        public override bool IsValid()
        {
            ValidationResult = new UpdateEntityCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}