using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class EntityAttributeRelationCommand : RelationCommand
    {       
        public override bool IsValid()
        {
            return true;
        }
    }
}