using Eaf.AutoMapper;
using System.Text.Json.Serialization;

namespace Eaf.Str.Airports.Jobs
{
    internal class AirportsJsonDto
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("content")]
        public ContentsJsonDto[] Content { get; set; }
    }

    [AutoMap(typeof(Airport))]
    internal class ContentsJsonDto
    {
        public string IATACode { get; set; }

        public string NamePT { get; set; }

        public string NameES { get; set; }

        public string NameEN { get; set; }

        public string ShortNamePT { get; set; }

        public string ShortNameES { get; set; }

        public string ShortNameEN { get; set; }

        public string ICAOCode { get; set; }

        public string CountryCode { get; set; }

        public string TimeZone { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}