using System.Collections.Generic;
using Eaf.Middleware.Dto;
using Eaf.Str.Airplanes.Dtos;

namespace Eaf.Str.Airplanes.Exporting
{
    public interface IAirplanesExcelExporter
    {
        FileDto ExportToFile(List<AirplaneDto> airplanes);
    }
}