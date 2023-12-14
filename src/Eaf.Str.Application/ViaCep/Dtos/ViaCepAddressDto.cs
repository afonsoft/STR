using Newtonsoft.Json;
using System;

namespace Eaf.Str.ViaCep.Dtos
{
    [Serializable]
    public class ViaCepAddressDto
    {
        [JsonProperty(PropertyName = "cep")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "logradouro")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "complemento")]
        public string Complement { get; set; }

        [JsonProperty(PropertyName = "bairro")]
        public string Neighborhood { get; set; }

        [JsonProperty(PropertyName = "localidade")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "uf")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "ibge")]
        public string IBGE { get; set; }

        [JsonProperty(PropertyName = "gia")]
        public string GIA { get; set; }

        [JsonProperty(PropertyName = "ddd")]
        public string DDD { get; set; }

        [JsonProperty(PropertyName = "siafi")]
        public string Siafi { get; set; }
    }
}