using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRMSystem.Application.Dtos.Account
{
    public class UpdateFighterDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string? Password { get; set; }
        public string? FinCode { get; set; }
    }
    public class UpdateFighterDtoValidation : AbstractValidator<UpdateFighterDto>
    {
        public UpdateFighterDtoValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname cannot be empty.");


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
