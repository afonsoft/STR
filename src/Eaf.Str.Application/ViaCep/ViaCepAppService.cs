using Eaf.Authorization;
using Eaf.Str.ViaCep.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eaf.Str.ViaCep
{
    [EafAllowAnonymous]
    public class ViaCepAppService : IViaCepAppService
    {
        private const string ZipCodeSizeErrorMessage = "Invalid ZipCode Size";
        private const string ZipCodeFormatErrorMessage = "Invalid ZipCode Format";

        private readonly HttpClient _httpClient;

        public ViaCepAppService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ViaCepHttpClient");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<ViaCepAddressDto> GetAddress(string zipCode)
        {
            var responseContent = await Execute(SetZipCodeUrl(zipCode));
            return JsonConvert.DeserializeObject<ViaCepAddressDto>(responseContent);
        }

        public async Task<IList<ViaCepAddressDto>> ListAddresses(string state, string city, string street)
        {
            var responseContent = await Execute(SetZipCodeUrlBy(state, city, street));
            return JsonConvert.DeserializeObject<IList<ViaCepAddressDto>>(responseContent);
        }

        private async Task<string> Execute(string url)
        {
            var responseMessage = await _httpClient.GetAsync(new Uri(url));
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsStringAsync();
        }

        private string SetZipCodeUrl(string zipCode) => $"https://viacep.com.br/ws/{ValidateZipCode(zipCode)}/json/";

        private string SetZipCodeUrlBy(string state, string city, string street) => $"https://viacep.com.br/ws/{ValidateParam("State", state, 2)}/{ValidateParam("City", city)}/{ValidateParam("Street", street)}/json/";

        private string ValidateParam(string name, string value, int size = 3)
        {
            var aux = value.Trim();

            if (string.IsNullOrEmpty(aux) || aux.Length < size)
            {
                throw new EafException($"Invalid {name}, parameter below size of {size} characters.");
            }

            return aux;
        }

        private string ValidateZipCode(string zipCode)
        {
            var zipAux = zipCode.Trim().Replace("-", "");

            if (zipAux.Length != 8)
            {
                throw new EafException(ZipCodeSizeErrorMessage);
            }

            if (!Regex.IsMatch(zipAux, ("[0-9]{8}")))
            {
                throw new EafException(ZipCodeFormatErrorMessage);
            }

            return zipAux;
        }
    }
}