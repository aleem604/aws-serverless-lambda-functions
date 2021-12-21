using System;
using TinCore.Domain.Commands;
using FluentValidation;

namespace TinCore.Domain.Validations
{
    public abstract class EntityObjectValidation<T> : AbstractValidator<T> where T : EntityObjectCommand
    {
        protected void ValidateISN()
        {
            RuleFor(c => c.isn)
                .NotEqual(0).WithMessage("Please ensure you have entered the isn");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.isn)
                .NotEqual(0);
        }

    }
}