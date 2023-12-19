using Eaf.Middleware.Dto;
using Eaf.Str.Awbs.Dtos;
using System.Collections.Generic;

namespace Eaf.Str.Awbs.Exporting
{
    public interface IAwbExcelExporter
    {
        FileDto ExportToFile(List<AwbDto> awbs);
    }
}