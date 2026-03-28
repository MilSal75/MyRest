#nullable disable
using MyRest.Domain.ValueObjects.Base;
using MyRest.Domain.ValueObjects.Validators;

namespace MyRest.Domain.ValueObjects
{
    public class Phone : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new PhoneValidator();

        public Phone()
        {
        }

        public Phone(string phone) : this(_defaultValidator, phone)
        {
        }

        public Phone(IValidator<string> validator, string phone) : base(validator, phone)
        {
        }
    }
}