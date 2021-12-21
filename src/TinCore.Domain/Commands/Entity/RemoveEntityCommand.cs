using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class RemoveEntityCommand : EntityCommand
    {
        public RemoveEntityCommand(int id)
        {
            this.Id = id;
            AggregateId = Guid.NewGuid();
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveEntityCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}