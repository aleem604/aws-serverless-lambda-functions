using System;
using TinCore.Domain.Validations;

namespace TinCore.Domain.Commands
{
    public class RelationEntityRelationCommand : RelationCommand
    {
        
        public override bool IsValid()
        {
            return true;            
        }
    }
}