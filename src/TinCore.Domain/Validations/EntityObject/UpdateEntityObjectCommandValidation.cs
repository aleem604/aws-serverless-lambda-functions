using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class UpdateEntityObjectCommandValidation : EntityObjectValidation<UpdateEntityObjectCommand>
    {
        public UpdateEntityObjectCommandValidation()
        {
            ValidateId();
            ValidateISN();
        }
    }
}