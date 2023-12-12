using System.Collections.Generic;
using Eaf.Str.Airplanes.Dtos;
using Eaf.Middleware.DataExporting.Excel.EpPlus;
using Eaf.Middleware.Storage;
using Eaf.Middleware.Dto;

namespace Eaf.Str.Airplanes.Exporting
{
    public class AirplanesExcelExporter : EpPlusExcelExporterBase, IAirplanesExcelExporter
    {
        public AirplanesExcelExporter(
            ITempFileCacheManager tempFileCacheManager
        ) :base(tempFileCacheManager)
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
        }

        public FileDto ExportToFile(List<AirplaneDto> airplanes)
        {
            return CreateExcelPackage(
                "Airplanes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Airplanes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Number"),
                        L("Model")
                        );

                    AddObjects(
                        sheet, 2, airplanes,
                        _ => _.Number,
                        _ => _.Model
                        );
                });
        }
    }
}
