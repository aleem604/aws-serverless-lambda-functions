using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class UpdateEntityCommandValidation : EntityValidation<UpdateEntityCommand>
    {
        public UpdateEntityCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}