using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class NewRelationCommandValidation : RelationValidation<RelationCommand>
    {
        public NewRelationCommandValidation()
        {
            ValidateName();
        }
    }
}