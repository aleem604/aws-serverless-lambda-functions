using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class LocationCategoryRelationCommand : RelationCommand
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}