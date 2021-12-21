using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class NewEntityObjectCommandValidation : EntityObjectValidation<NewEntityObjectCommand>
    {
        public NewEntityObjectCommandValidation()
        {
            ValidateISN();
        }
    }
}