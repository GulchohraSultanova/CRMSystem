using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Account
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Password { get; set; }
        public string? FinCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname cannot be empty.");

            RuleFor(x => x.PhoneNumber)
           .NotEmpty().WithMessage("Telefon nömrəsi boş ola bilməz.")
           .Matches(@"^(\+994|0)(50|51|55|70|77|10|99|12|60)\d{7}$")
           .WithMessage("Telefon nömrəsi düzgün formatda olmalıdır (məs: 0501234567 və ya +994501234567).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password cannot be empty!")
                .Must(r =>
                {
                    Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,50}$");
                    return passwordRegex.IsMatch(r);
                }).WithMessage("Password format is not correct!")
                .Must(p => !p.Contains(" ")).WithMessage("The password cannot contain spaces!");




        }
    }
}
