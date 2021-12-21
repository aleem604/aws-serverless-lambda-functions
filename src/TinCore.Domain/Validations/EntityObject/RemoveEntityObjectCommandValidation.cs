using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class RemoveEntityObjectCommandValidation : EntityObjectValidation<RemoveEntityObjectCommand>
    {
        public RemoveEntityObjectCommandValidation()
        {
            ValidateId();
        }
    }
}