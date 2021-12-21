using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class RemoveRelationCommandValidation : RelationValidation<RelationCommand>
    {
        public RemoveRelationCommandValidation()
        {
            ValidateId();
        }
    }
}