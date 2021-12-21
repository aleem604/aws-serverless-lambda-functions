using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class LocationEntityRelationCommand : RelationCommand
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}