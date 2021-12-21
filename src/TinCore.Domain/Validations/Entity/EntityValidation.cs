using System;
using TinCore.Domain.Commands;
using FluentValidation;

namespace TinCore.Domain.Validations
{
    public abstract class EntityValidation<T> : AbstractValidator<T> where T : EntityCommand
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