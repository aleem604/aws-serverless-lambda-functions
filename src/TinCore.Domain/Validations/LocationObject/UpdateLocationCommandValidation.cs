using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class UpdateLocationCommandValidation : LocationValidation<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidation()
        {
            ValidateId();
        }
    }
}