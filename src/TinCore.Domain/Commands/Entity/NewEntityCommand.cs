using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class NewEntityCommand : EntityCommand
    {
        
        public override bool IsValid()
        {
            ValidationResult = new NewEntityCommandValidation().Validate(this);
            return ValidationResult.IsValid;            
        }
    }
}