using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class RemoveEntityCommandValidation : EntityValidation<RemoveEntityCommand>
    {
        public RemoveEntityCommandValidation()
        {
            ValidateId();
        }
    }
}