using TinCore.Domain.Commands;

namespace TinCore.Domain.Validations
{
    public class NewLocationCommandValidation : LocationValidation<NewLocationCommand>
    {
        public NewLocationCommandValidation()
        {
            ValidateLocationType();
        }
    }
}