using CRMSystem.Application.Dtos.Account;
using   CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface ITokenService
    {
        TokenResponseDto CreateToken(Admin admin, string role, int expireDate = 1440);
    }
}
