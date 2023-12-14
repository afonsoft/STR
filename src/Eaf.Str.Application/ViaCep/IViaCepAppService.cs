using Eaf.Application.Services;
using Eaf.Str.ViaCep.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaf.Str.ViaCep
{
    public interface IViaCepAppService : IApplicationService
    {
        Task<ViaCepAddressDto> GetAddress(string zipCode);

        Task<IList<ViaCepAddressDto>> ListAddresses(string state, string city, string street);
    }
}