using System;
using TinCore.Domain.Commands;
using FluentValidation;

namespace TinCore.Domain.Validations
{
    public abstract class RelationValidation<T> : AbstractValidator<T> where T : RelationCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty();
        }
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(0);
        }

    }
}