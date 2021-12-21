using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class RemoveLocationCommandValidation : LocationValidation<RemoveLocationCommand>
    {
        public RemoveLocationCommandValidation()
        {
            ValidateId();
        }
    }
}