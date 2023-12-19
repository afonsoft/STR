using Eaf.Middleware.DataExporting.Excel.EpPlus;
using Eaf.Middleware.Dto;
using Eaf.Middleware.Storage;
using Eaf.Str.Awbs.Dtos;
using System.Collections.Generic;

namespace Eaf.Str.Awbs.Exporting
{
    internal class AwbExcelExporter : EpPlusExcelExporterBase, IAwbExcelExporter
    {
        public AwbExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
        }

        public FileDto ExportToFile(List<AwbDto> awbs)
        {
            return CreateExcelPackage(
              "Awb.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.Workbook.Worksheets.Add(L("AWB"));
                  sheet.OutLineApplyStyle = true;

                  AddHeader(
                      sheet,
                      L("Number"),
                      L("Model")
                      );

                  AddObjects(
                      sheet, 2, awbs,
                      _ => _.Origin,
                      _ => _.Destiny,
                      _ => _.TrackingNumber,
                      _ => _.Sender.PersonName,
                      _ => _.Sender.Street,
                      _ => _.Recipient.PersonName,
                      _ => _.Recipient.Street
                      );
              });
        }
    }
}