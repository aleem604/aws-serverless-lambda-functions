using System;
using TinCore.Domain.Commands;
using FluentValidation;

namespace TinCore.Domain.Validations
{
    public abstract class LocationValidation<T> : AbstractValidator<T> where T : LocationCommand
    {        
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(0);
        }
        protected void ValidateLocationType()
        {
            RuleFor(c => c.LocationType)
                .NotEqual(null);
        }

    }
}