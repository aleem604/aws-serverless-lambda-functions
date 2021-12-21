using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class UpdateRelationCommandValidation : RelationValidation<RelationCommand>
    {
        public UpdateRelationCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}