global using OfficeOpenXml;

namespace TimeSeriesCommon;

public class TimeSeriesLoader
{
    private const string outputSheetName = "output";
    private const string OutputPath = "C:\\Users\\Admin\\source\\repos\\TimeSeriesFixer\\TimeSeriesFixer\\output.xlsx";
    public static async Task<List<TimeSeriesEntry>> LoadTimeSeries(FileInfo inputFile, bool hasHeaders)
    {
        using var package = new ExcelPackage(inputFile);
        await package.LoadAsync(inputFile);
        var ws = package.Workbook.Worksheets[0];
        int row = hasHeaders ? 2 : 1, col = 1;
        List<TimeSeriesEntry> result = new();
        while (string.IsNullOrEmpty(ws.Cells[row, col].Value?.ToString()) == false)
        {
            try
            {
                string date = ws.Cells[row, col].Value.ToString();
                decimal value = decimal.Parse(ws.Cells[row, col + 1].Value.ToString());

                result.Add(new(date, value));
                row++;
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while parsing at row {row} and col {col}");
                package.Dispose();
                throw;
            }
            
        }

        return result;
    }
    public static async Task SaveTimeSeries(List<TimeSeriesEntry> entries, bool hasHeaders)
    {
        FileInfo outputFile = new(OutputPath);
        DeleteIfExists(outputFile);

        using var package = new ExcelPackage(outputFile);
        var ws = package.Workbook.Worksheets.Add(outputSheetName);
        var range = ws.Cells["A2"].LoadFromCollection(entries, hasHeaders);
        range.AutoFitColumns();
        await package.SaveAsync();
    }

    private static void DeleteIfExists(FileInfo outputFile)
    {
        if (outputFile.Exists)
        {
            outputFile.Delete();
        }
    }
}
