using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class NewEntityCommandValidation : EntityValidation<NewEntityCommand>
    {
        public NewEntityCommandValidation()
        {
            ValidateName();
        }
    }
}